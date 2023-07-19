using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.Features.Commands.AppUser.CreateUser;
using MiniETicaret.Application.Features.Commands.AppUser.FacebookLogin;
using MiniETicaret.Application.Features.Commands.AppUser.GoogleLogin;
using MiniETicaret.Application.Features.Commands.AppUser.LoginUser;
using MiniETicaret.Application.Features.Commands.AppUser.UpdateForgetPassword;

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

    }
}
