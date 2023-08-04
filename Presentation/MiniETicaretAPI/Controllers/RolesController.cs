using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniETicaret.Application.CustomAttributes;
using MiniETicaret.Application.Enums;
using MiniETicaret.Application.Features.Commands.Role.CreateRole;
using MiniETicaret.Application.Features.Commands.Role.DeleteRole;
using MiniETicaret.Application.Features.Commands.Role.UpdateRole;
using MiniETicaret.Application.Features.Queries.Role.GetRoleById;
using MiniETicaret.Application.Features.Queries.Role.GetRoles;

namespace MiniETicaretAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class RolesController : ControllerBase
    {
        readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get All Roles")]
        public async Task<IActionResult> GetRolesAsync([FromQuery]GetRolesQueryRequest getRolesQueryRequest)
        {
            GetRolesQueryResponse response = await _mediator.Send(getRolesQueryRequest);
            return Ok(response);
        }
        
        
        [HttpGet("{roleId}")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Role By Id")]
        public async Task<IActionResult> GetRoleByIdAsync([FromRoute]GetRoleByIdQueryRequest getRoleByIdQueryRequest)
        {
            GetRoleByIdQueryResponse response = await _mediator.Send(getRoleByIdQueryRequest);
            return Ok(response);
        }
        
        [HttpPost()]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Create Role")]
        public async Task<IActionResult> CreateRoleAsync([FromBody]CreateRoleCommandRequest createRoleCommandRequest)
        {
            CreateRoleCommandResponse response = await _mediator.Send(createRoleCommandRequest);
            return Ok(response);
        }
        
        [HttpPut("{roleId}")]
        [AuthorizeDefinition(ActionType = ActionType.Updating, Definition = "Update Role")]
        public async Task<IActionResult> UpdateRoleAsync([FromBody,FromRoute]UpdateRoleCommandRequest updateRoleCommandRequest)
        {
            UpdateRoleCommandResponse response = await _mediator.Send(updateRoleCommandRequest);
            return Ok(response);
        }
        
        [HttpDelete("{roleId}")]
        [AuthorizeDefinition(ActionType = ActionType.Deleting, Definition = "Delete Role")]
        public async Task<IActionResult> DeleteRoleAsync([FromRoute]DeleteRoleCommandRequest deleteRoleCommandRequest)
        {
            DeleteRoleCommandResponse response = await _mediator.Send(deleteRoleCommandRequest);
            return Ok(response);
        }
    }
}
