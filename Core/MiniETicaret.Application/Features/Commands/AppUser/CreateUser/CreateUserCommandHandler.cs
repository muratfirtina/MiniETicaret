using MediatR;
using Microsoft.AspNetCore.Identity;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.DTOs.User;
using MiniETicaret.Application.Exceptions;

namespace MiniETicaret.Application.Features.Commands.AppUser.CreateUser;

public class CreateUserCommandHandler:IRequestHandler<CreateUserCommandRequest,CreateUserCommandResponse>
{
    readonly IUserService _userService;

    public CreateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
    {
        CreateUserResponse response = await _userService.CreateAsync(new()
        {
            NameSurname = request.NameSurname,
            UserName = request.UserName,
            Email = request.Email,
            Password = request.Password,
            ConfirmPassword = request.ConfirmPassword
        });
       
       return new ()
       {
           Message = response.Message,
           IsSuccess = response.IsSuccess,
       };
       
    }
}
