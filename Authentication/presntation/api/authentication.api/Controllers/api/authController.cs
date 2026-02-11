using authentication.application.DTO;
using authentication.application.Feature.authfeature.request.Commands;
using authentication.application.Feature.authfeature.request.Queries;
using authentication.domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace authentication.api.Controllers.api
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class authController : ControllerBase
    {
        private readonly IMediator _mediator;

        public authController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> register([FromBody] registerDTO myregisterDTO)
        {
            var command = new createAuthCommand
            {
                registerdto = myregisterDTO
            };
            var response=await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody]loginDTO mylogin)
        {
            var response =await _mediator.Send(new loginRequest
            {
                myslogindto=mylogin
            });
            return Ok(response);
        }

        [HttpPost("CreateRole/{roleName}")]
        //[Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return BadRequest("Role name should be provided.");
            }

            var newRole = new Role
            {
                Name = roleName
            };
            var command = new createRoleCommand
            {
                RoleName = roleName
            };
            var roleResult = _mediator.Send(command);

            if (roleResult.Result.Success==true)
            {
                return Ok(roleResult.Result.Message);
            }

            return Problem(roleResult.Result.Errors.FirstOrDefault(), null, 500);
        }
    }
}
