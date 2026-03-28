using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi.Model;
using webapi.Model.Authentication;
using webapi.Services.IServices.Identity;

namespace webapi.Controllers.api.authentication
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class authController : ControllerBase
    {
        private readonly IauthIdentityService _service;

        public authController(IauthIdentityService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> register([FromBody] registerDTO dTO)
        {
            dTO.Role = "Customer";
            var response = await _service.register<ResponseDTO>(dTO);
            if (response.IsSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(false);
        }
        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] loginDTO dTO)
        {
            var response = await _service.login<ResponseDTO>(dTO);
            if (response.IsSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.ErrorMessages);
        }

        [HttpPost("CreateRole/{name}")]
        public async Task<IActionResult> CreateRole(string name)
        {
            var response = await _service.CreateRole<ResponseDTO>(name);
            if (response.IsSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }
    }
}
