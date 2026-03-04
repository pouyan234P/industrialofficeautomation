using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Xml.Linq;
using webapi.Model;
using webapi.Model.Authentication;
using webapi.Services.IServices.Identity;

namespace webapi.Controllers.api.authentication
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class positionController : ControllerBase
    {
        private readonly IpositionIdentityService _service;
        private readonly IdepartmentIdentityService _service1;

        public positionController(IpositionIdentityService service,IdepartmentIdentityService service1)
        {
            _service = service;
            _service1 = service1;
        }

        [HttpPost("insertPosition")]
        public async Task<IActionResult> insertPosition([FromBody] setPosition setPosition)
        {
           
            var response=await _service.insertPosition<ResponseDTO>(setPosition);
            if(response.IsSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.ErrorMessages);
        }

        [HttpGet("getPosition/{id}")]
        public async Task<IActionResult> getPosition(int id)
        {
            var response=await _service.getPosition<ResponseDTO>(id);
            if(response.IsSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.ErrorMessages);
        }
    }
}
