using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using webapi.Model;
using webapi.Model.Authentication;
using webapi.Services.IServices.Identity;

namespace webapi.Controllers.api.authentication
{
    /// <summary>Role management for the Angular Roles page. Admin-only.</summary>
    [Authorize(Policy = Policies.RequireAdminRole)]
    [Route("api/[controller]")]
    [ApiController]
    public class roleController : ControllerBase
    {
        private readonly IauthIdentityService _service;

        public roleController(IauthIdentityService service)
        {
            _service = service;
        }

        /// <summary>GET api/role/getAll — every role with how many users hold it.</summary>
        [HttpGet("getAll")]
        public async Task<IActionResult> getAll()
        {
            var response = await _service.getRoles<ResponseDTO>();
            if (response.IsSuccess)
            {
                var roles = response.Result == null
                    ? new List<roleDTO>()
                    : JsonConvert.DeserializeObject<List<roleDTO>>(Convert.ToString(response.Result)!) ?? new List<roleDTO>();
                return Ok(roles);
            }
            return BadRequest(response.ErrorMessages);
        }

        /// <summary>POST api/role/create — body { "name": "Manager" }.</summary>
        [HttpPost("create")]
        public async Task<IActionResult> create([FromBody] createRoleDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto?.name))
                return BadRequest(new List<string> { "Role name is required." });

            var response = await _service.createRole<ResponseDTO>(dto);
            if (response.IsSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.ErrorMessages);
        }
    }
}
