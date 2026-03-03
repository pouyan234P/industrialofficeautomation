using authentication.api.DTO;
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
        protected ResponseDTO _response;
        private readonly IMediator _mediator;

        public authController(IMediator mediator)
        {
            _mediator = mediator;
            this._response = new ResponseDTO();
        }

        [HttpPost("register")]
        public async Task<IActionResult> register([FromBody] registerDTO myregisterDTO)
        {
            try
            {
                var command = new createAuthCommand
                {
                    registerdto = myregisterDTO
                };
                var response = await _mediator.Send(command);
                _response.Result = response;
            }
            catch (Exception e)
            {
                _response.IsSuccess=false;
                _response.ErrorMessages=new List<string>() { e.ToString()};
            }
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody]loginDTO mylogin)
        {
            try
            {
                var response = await _mediator.Send(new loginRequest
                {
                    myslogindto = mylogin
                });
                _response.Result=response;
            }
            catch (Exception e)
            {
                _response.IsSuccess=false;
                _response.ErrorMessages= new List<string>() { e.ToString()};
            }
            return Ok(_response);
        }

        [HttpPost("CreateRole/{roleName}")]
        //[Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(roleName))
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>() { "Role name should be provided" };
                    return Ok(_response);
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

                if (roleResult.Result.Success == true)
                {
                    _response.Result = roleResult.Result.Message;
                    return Ok(_response);
                }
                return Problem(roleResult.Result.Errors.FirstOrDefault(), null, 500);
            }
            catch (Exception e)
            {
                _response.IsSuccess=false;
                _response.ErrorMessages=new List<string>() { e.ToString()};
            }
            return Ok(_response);
        }
    }
}
