using Microsoft.AspNetCore.Identity;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.DTOs.User;
using MiniETicaret.Domain.Entities.Identity;

namespace MiniETicaret.Persistence.Services;

public class UserService:IUserService
{
    readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;

    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<CreateUserResponse> CreateAsync(CreateUser model)
    {
        IdentityResult result = await _userManager.CreateAsync(new ()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = model.Email,
                    NameSurname = model.NameSurname,
                    UserName = model.UserName
                }, model.Password);
               
               CreateUserResponse response = new(){ IsSuccess = result.Succeeded };
               if (result.Succeeded)
                   response.Message = "Kullanıcı başarıyla oluşturuldu."; //todo: dil desteği eklenecek
               else
                   foreach (var error in result.Errors)
                       response.Message += $"{error.Code} - {error.Description}\n";
               return response;
               
    }
}