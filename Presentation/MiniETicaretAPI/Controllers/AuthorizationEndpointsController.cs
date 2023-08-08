using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniETicaret.Application.Features.Commands.AuthorizationEndpoint.AssignRoleToEndpoint;
using MiniETicaret.Application.Features.Queries.AuthorizationEndpoint;

namespace MiniETicaretAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationEndpointsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorizationEndpointsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost("get-roles-to-endpoint")]
        public async Task<IActionResult> GetRolesToEndpoint([FromBody]GetRolesToEndpointQueryRequest getRolesToEndpointQueryRequest)
        {
            var response = await _mediator.Send(getRolesToEndpointQueryRequest);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRoleToEndpoint(AssignRoleToEndpointCommandRequest assignRoleToEndpointCommandRequest)
        {
            assignRoleToEndpointCommandRequest.Type = typeof(Program);
            var response = await _mediator.Send(assignRoleToEndpointCommandRequest);
            return Ok(response);
        }
    }
}
