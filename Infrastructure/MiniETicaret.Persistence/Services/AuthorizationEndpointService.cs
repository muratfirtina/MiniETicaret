using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.Abstractions.Services.Configurations;
using MiniETicaret.Application.DTOs.Configuration;
using MiniETicaret.Application.DTOs.Role;
using MiniETicaret.Application.Repositories;
using MiniETicaret.Domain.Entities;
using MiniETicaret.Domain.Entities.Identity;

namespace MiniETicaret.Persistence.Services;

public class AuthorizationEndpointService: IAuthorizationEndpointService
{
    readonly IApplicationService _applicationService;
    readonly IEndpointReadRepository _endpointReadRepository;
    readonly IEndpointWriteRepository _endpointWriteRepository;
    readonly IAcMenuReadRepository _acMenuReadRepository;
    readonly IAcMenuWriteRepository _acMenuWriteRepository;
    readonly RoleManager<AppRole> _roleManager;


    public AuthorizationEndpointService(IApplicationService applicationService, IEndpointReadRepository endpointReadRepository, IEndpointWriteRepository endpointWriteRepository, IAcMenuReadRepository acMenuReadRepository, IAcMenuWriteRepository acMenuWriteRepository, RoleManager<AppRole> roleManager)
    {
        _applicationService = applicationService;
        _endpointReadRepository = endpointReadRepository;
        _endpointWriteRepository = endpointWriteRepository;
        _acMenuReadRepository = acMenuReadRepository;
        _acMenuWriteRepository = acMenuWriteRepository;
        _roleManager = roleManager;
    }

    public async Task AssignRoleToEndpointAsync(List<RoleDto> roles, string menu, string code, Type type)
    {
        bool _menu = await _acMenuReadRepository.ExistAsync(x => x.Name == menu);
        if (!_menu)
        {
            await _acMenuWriteRepository.AddAsync(new()
            {
                Id = Guid.NewGuid(),
                Name = menu,
            });
            await _acMenuWriteRepository.SaveAsync();
        }
        ACMenu acMenu = await _acMenuReadRepository.GetSingleAsync(x => x.Name == menu);

        bool _endpoint = await _endpointReadRepository.ExistAsync(x => x.AcMenu == acMenu && x.Code == code);
        if (!_endpoint)
        {
            var action = _applicationService.GetAuthorizeDefinitionEnpoints(type)
                .FirstOrDefault(m=>m.Name == menu)?.Actions.FirstOrDefault(a => a.Code == code);
            
            await _endpointWriteRepository.AddAsync(new()
            {
                Id = Guid.NewGuid(),
                Code = code,
                ActionType = action.ActionType,
                HttpType = action.HttpType,
                Definition = action.Definition,
                AcMenu = acMenu
                
            });
            await _endpointWriteRepository.SaveAsync();
        }
        
        Endpoint? endpoint = await _endpointReadRepository.Table
            .Include(e => e.AcMenu)
            .Include(e => e.Roles)
            .FirstOrDefaultAsync(e => e.Code == code && e.AcMenu.Name == menu );

        foreach (var role in endpoint?.Roles)
            endpoint.Roles.Remove(role);

        var selectedRoles = roles.Select(x => x.RoleName).ToList();
        foreach (var roleName in selectedRoles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                endpoint.Roles.Add(role);
            }
        }
        
        await _endpointWriteRepository.SaveAsync();
        
    }

    public async Task<List<RoleDto>?> GetRolesToEndpointAsync(string code, string menu)
    {
       Endpoint? endpoint = await _endpointReadRepository.Table
            .Include(e => e.AcMenu)
            .Include(e => e.Roles)
            .FirstOrDefaultAsync(e => e.Code == code && e.AcMenu.Name == menu);
       
        return endpoint?.Roles.Select(x => new RoleDto()
        {
            RoleName = x.Name,
            RoleId = x.Id.ToString()
        }).ToList();
    }
}