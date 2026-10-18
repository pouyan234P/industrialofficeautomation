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

        /// <summary>Creates a user. Any signed-in user may do this; only an Admin may choose the new user's role.</summary>
        [HttpPost("register")]
        public async Task<IActionResult> register([FromBody] registerDTO dTO)
        {
            // Non-admins always create Customers (putting someone in a role is Admin-only).
            if (string.IsNullOrWhiteSpace(dTO.Role) || !User.IsInRole(Policies.AdminRole))
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
