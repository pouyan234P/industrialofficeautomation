using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Model;
using webapi.Model.Authentication;
using webapi.Services.IServices.Identity;

namespace webapi.Controllers.api.authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class authController : ControllerBase
    {
        private readonly IauthIdentityService _service;

        public authController(IauthIdentityService service)
        {
            _service = service;
        }

        /// <summary>Creates a user. Admin-only: anonymous self-registration was a way in for anyone.</summary>
        [Authorize(Policy = Policies.RequireAdminRole)]
        [HttpPost("register")]
        public async Task<IActionResult> register([FromBody] registerDTO dTO)
        {
            // The admin may pick a role on the New User form; default to Customer.
            if (string.IsNullOrWhiteSpace(dTO.Role))
                dTO.Role = "Customer";

            var response = await _service.register<ResponseDTO>(dTO);
            if (response.IsSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.ErrorMessages);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] loginDTO dTO)
        {
            var response = await _service.login<ResponseDTO>(dTO);
            if (response.IsSuccess && response.Result is not null)
            {
                return Ok(new { token = response.Result });
            }
            return BadRequest(response.ErrorMessages);
        }

        /// <summary>Kept for older callers; the Roles page uses POST api/role/create.</summary>
        [Authorize(Policy = Policies.RequireAdminRole)]
        [HttpPost("CreateRole/{name}")]
        public async Task<IActionResult> CreateRole(string name)
        {
            var response = await _service.CreateRole<ResponseDTO>(name);
            if (response.IsSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.ErrorMessages);
        }
    }
}
