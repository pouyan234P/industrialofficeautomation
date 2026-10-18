using authentication.api.DTO;
using authentication.application;
using authentication.application.DTO;
using authentication.application.Feature.authfeature.request.Commands;
using authentication.application.Feature.authfeature.request.Queries;
using authentication.application.Responses;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace authentication.api.Controllers.api
{
    /// <summary>
    /// Login is public. Any signed-in user may create, list and edit users.
    /// Creating roles and putting people in roles is Admin-only; when a non-admin
    /// creates a user, the new account always gets the default Customer role.
    /// </summary>
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

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] loginDTO mylogin)
        {
            try
            {
                var result = await _mediator.Send(new loginRequest { myslogindto = mylogin });
                if (result.Success)
                {
                    _response.Result = result.Token;
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { result.Error ?? "Login failed." };
                }
            }
            catch (ValidationException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = ex.Errors.Select(e => e.ErrorMessage).ToList();
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = Errors.Unexpected(HttpContext, e);
            }
            return Ok(_response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> register([FromBody] registerDTO myregisterDTO)
        {
            // Choosing a role = adding someone to a role, which only admins may do.
            if (myregisterDTO != null && !User.IsInRole(AppRoles.Admin))
                myregisterDTO.Role = AppRoles.Customer;

            return await Run(async () =>
            {
                var result = await _mediator.Send(new createAuthCommand { registerdto = myregisterDTO });
                ApplyCommandResult(result);
            });
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> getAll()
        {
            return await Run(async () =>
            {
                _response.Result = await _mediator.Send(new getAllRequest());
            });
        }

        [HttpPut("updateUser/{id}")]
        public async Task<IActionResult> updateUser(int id, [FromBody] updateUserDTO dto)
        {
            return await Run(async () =>
            {
                var result = await _mediator.Send(new updateUserCommand
                {
                    id = id,
                    userDTO = dto,
                    callerIsAdmin = User.IsInRole(AppRoles.Admin),
                });
                ApplyCommandResult(result);
            });
        }

        // ─── Roles ───────────────────────────────────────────────────────────

        [Authorize(Policy = Policies.RequireAdminRole)]
        [HttpGet("roles")]
        public async Task<IActionResult> getRoles()
        {
            return await Run(async () =>
            {
                _response.Result = await _mediator.Send(new getRolesRequest());
            });
        }

        [Authorize(Policy = Policies.RequireAdminRole)]
        [HttpPost("roles")]
        public async Task<IActionResult> createRole([FromBody] createRoleDTO dto)
        {
            return await Run(async () =>
            {
                var result = await _mediator.Send(new createRoleCommand { RoleName = dto?.name });
                ApplyCommandResult(result);
            });
        }

        /// <summary>Kept for older callers; prefer POST api/auth/roles with a JSON body.</summary>
        [Authorize(Policy = Policies.RequireAdminRole)]
        [HttpPost("CreateRole/{roleName}")]
        public Task<IActionResult> CreateRole(string roleName)
            => createRole(new createRoleDTO { name = roleName });

        [Authorize(Policy = Policies.RequireAdminRole)]
        [HttpPut("users/{id}/roles")]
        public async Task<IActionResult> setUserRoles(int id, [FromBody] setUserRolesDTO dto)
        {
            return await Run(async () =>
            {
                var result = await _mediator.Send(new setUserRolesCommand
                {
                    userId = id,
                    roles = dto?.roles,
                    actingUserId = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var me) ? me : null,
                });
                ApplyCommandResult(result);
            });
        }

        // ─── helpers ─────────────────────────────────────────────────────────

        private void ApplyCommandResult(baseCommandResponse result)
        {
            _response.Result = result;
            if (!result.Success)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = result.Errors?.Count > 0
                    ? result.Errors
                    : new List<string> { result.Message ?? "The operation failed." };
            }
        }

        private async Task<IActionResult> Run(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (ValidationException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = ex.Errors.Select(e => e.ErrorMessage).ToList();
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = Errors.Unexpected(HttpContext, e);
            }
            return Ok(_response);
        }
    }
}
