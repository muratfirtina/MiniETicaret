using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniETicaret.Application.Abstractions.Services.Configurations;
using MiniETicaret.Application.Consts;
using MiniETicaret.Application.CustomAttributes;
using MiniETicaret.Application.DTOs.Configuration;
using MiniETicaret.Application.Enums;

namespace MiniETicaretAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    
    public class ApplicationServicesController : ControllerBase
    {
        readonly IApplicationService _applicationService;

        public ApplicationServicesController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }
        [HttpGet]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Authorize Definition Enpoints")]
        public IActionResult GetAuthorizeDefinitionEnpoints()
        {
            var datas = _applicationService.GetAuthorizeDefinitionEnpoints(typeof(Program));
            return Ok(datas);
        }
        
    }
}
