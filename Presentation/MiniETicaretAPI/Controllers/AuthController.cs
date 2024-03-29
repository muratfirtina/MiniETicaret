using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniETicaret.Application.Features.Commands.AppUser.FacebookLogin;
using MiniETicaret.Application.Features.Commands.AppUser.GoogleLogin;
using MiniETicaret.Application.Features.Commands.AppUser.LoginUser;
using MiniETicaret.Application.Features.Commands.AppUser.PasswordReset;
using MiniETicaret.Application.Features.Commands.AppUser.RefreshTokenLogin;
using MiniETicaret.Application.Features.Commands.AppUser.VerifyResetPasswordToken;

namespace MiniETicaretAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginUserCommandRequest loginUserCommandRequest)
        {
            var response = await _mediator.Send(loginUserCommandRequest);
            return Ok(response);
        }
        
        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshTokenLogin([FromBody]RefreshTokenLoginCommandRequest refreshTokenLoginCommandRequest)
        {
            var response = await _mediator.Send(refreshTokenLoginCommandRequest);
            return Ok(response);
        }
        

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginCommandRequest googleLoginCommandRequest)
        {
            var response = await _mediator.Send(googleLoginCommandRequest);
            return Ok(response);
        }
        
        [HttpPost("facebook-login")]
        public async Task<IActionResult> FacebookLogin(FacebookLoginCommandRequest facebookLoginCommandRequest)
        {
            var response = await _mediator.Send(facebookLoginCommandRequest);
            return Ok(response);
        }

        [HttpPost("password-reset")]
        public async Task<IActionResult> PasswordReset(PasswordResetCommandRequest passwordResetCommandRequest)
        {
            var response = await _mediator.Send(passwordResetCommandRequest);
            return Ok(response);
        }
        
        [HttpPost("verify-reset-password-token")]
        public async Task<IActionResult> VerifyResetPasswordToken([FromBody]VerifyResetPasswordTokenCommandRequest verifyResetPasswordTokenCommandRequest)
        {
            var response = await _mediator.Send(verifyResetPasswordTokenCommandRequest);
            return Ok(response);
        }
        
    }
}
