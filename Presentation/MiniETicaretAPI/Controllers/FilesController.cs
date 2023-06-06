using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MiniETicaretAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class FilesController : ControllerBase
    {
        readonly IConfiguration _configuration;

        public FilesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("[action]")]
        public IActionResult GetBaseStorageUrl()     
        {
            return Ok(new
            {
                Url = _configuration["BaseStorageUrl"]
            });
        }
        
    }
}
