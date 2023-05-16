using MediatR;
using Microsoft.AspNetCore.Identity;
using MiniETicaret.Application.Exceptions;

namespace MiniETicaret.Application.Features.Commands.AppUser.CreateUser;

public class CreateUserCommandHandler:IRequestHandler<CreateUserCommandRequest,CreateUserCommandResponse>
{
    readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;

    public CreateUserCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
    {
       IdentityResult result = await _userManager.CreateAsync(new ()
        {
            Id = Guid.NewGuid().ToString(),
            Email = request.Email,
            NameSurname = request.NameSurname,
            UserName = request.UserName
        }, request.Password);
       
       CreateUserCommandResponse response = new(){ IsSuccess = result.Succeeded };
       if (result.Succeeded)
           response.Message = "Kullanıcı başarıyla oluşturuldu.";
       else
           foreach (var error in result.Errors)
               response.Message += $"{error.Code} - {error.Description}\n";
       
       return response;
       
    }
}
