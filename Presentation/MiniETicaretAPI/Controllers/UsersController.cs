using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.Consts;
using MiniETicaret.Application.CustomAttributes;
using MiniETicaret.Application.Enums;
using MiniETicaret.Application.Features.Commands.AppUser.AssignRoleToUser;
using MiniETicaret.Application.Features.Commands.AppUser.CreateUser;
using MiniETicaret.Application.Features.Commands.AppUser.FacebookLogin;
using MiniETicaret.Application.Features.Commands.AppUser.GoogleLogin;
using MiniETicaret.Application.Features.Commands.AppUser.LoginUser;
using MiniETicaret.Application.Features.Commands.AppUser.UpdateForgetPassword;
using MiniETicaret.Application.Features.Queries.AppUser;
using MiniETicaret.Application.Features.Queries.AppUser.GetAllUsers;
using MiniETicaret.Application.Features.Queries.AppUser.GetRolesToUser;

namespace MiniETicaretAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        readonly IMediator _mediator;
        readonly IMailService _mailService;
        public UsersController(IMediator mediator, IMailService mailService)
        {
            _mediator = mediator;
            _mailService = mailService;
        }
        
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = ActionType.Reading,Definition = "Get All Users")]
        public async Task<IActionResult> GetAllUsers([FromQuery]GetAllUsersQueryRequest getAllUsersQueryRequest)
        {
            var response = await _mediator.Send(getAllUsersQueryRequest);
            return Ok(response);
        }
        

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommandRequest createUserCommandRequest)
        {
            var response = await _mediator.Send(createUserCommandRequest);
            return Ok(response);
        }
        
        [HttpPost("update-forgot-password")]
        public async Task<IActionResult>UpdateForgotPassword(UpdateForgotPasswordCommandRequest updateForgotPasswordCommandRequest)
        {
            var response = await _mediator.Send(updateForgotPasswordCommandRequest);
            return Ok(response);
        }
        
        [HttpPost("assign-role-to-user")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = ActionType.Updating,Definition = "Assign Role To User")]
        public async Task<IActionResult> AssignRoleToUser(AssignRoleToUserCommandRequest assignRoleToUserCommandRequest)
        {
            var response = await _mediator.Send(assignRoleToUserCommandRequest);
            return Ok(response);
        }
        
        [HttpGet("get-roles-to-user/{UserId}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Roles To Users")]
        public async Task<IActionResult> GetRolesToUser([FromRoute]GetRolesToUserQueryRequest getRolesToUserQueryRequest)
        {
            var response = await _mediator.Send(getRolesToUserQueryRequest);
            return Ok(response);
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginUserCommandRequest loginUserCommandRequest)
        {
            var response = await _mediator.Send(loginUserCommandRequest);
            return Ok(response);
        }
        
        [HttpPost("facebook-login")]
        public async Task<IActionResult> FacebookLogin(FacebookLoginCommandRequest facebookLoginCommandRequest)
        {
            var response = await _mediator.Send(facebookLoginCommandRequest);
            return Ok(response);
        }
        
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginCommandRequest googleLoginCommandRequest)
        {
            var response = await _mediator.Send(googleLoginCommandRequest);
            return Ok(response);
        }

    }
}
