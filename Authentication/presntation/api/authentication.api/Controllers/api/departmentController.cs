using authentication.api.DTO;
using authentication.application.DTO;
using authentication.application.Feature.departmentFeature.request.Commands;
using authentication.application.Feature.departmentFeature.request.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace authentication.api.Controllers.api
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class departmentController : ControllerBase
    {
        protected ResponseDTO _response;
        private readonly IMediator _mediator;

        public departmentController(IMediator mediator)
        {
            _mediator = mediator;
            this._response = new ResponseDTO();
        }

        [HttpPost("addDepartment")]
        public async Task<IActionResult> addDepartment([FromBody] DepartmentDTO mydepartmentDTO)
        {
            try
            {
                var command = new createDepartmentCommand
                {
                    departmentDTO = mydepartmentDTO
                };
                var result =await _mediator.Send(command);
                _response.Result = result;

            }
            catch (ValidationException ex)                              // ← catch this first
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = ex.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();
            }
            catch (Exception e)                                         // ← fallback stays
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return Ok(_response);
        }

        [HttpGet("getDepartmentbyid/{id}")]
        public async Task<IActionResult> getDepartmentbyid(int id)
        {
            try
            {
                var result = await _mediator.Send(new GetDepartmentbyidrequest
                {
                    Id = id
                });
                _response.Result = result;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages=new List<string> { e.ToString() };
            }
            return Ok(_response);
        }

        [HttpGet("getDepartment")]
        public async Task<IActionResult> getDepartment()
        {
            try
            {
                var result = await _mediator.Send(new GetDepartmentrequest());
                _response.Result = result;
            }
            catch (Exception e)
            {
                _response.IsSuccess=false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return Ok(_response);
        }

        [HttpGet("getDepartmentbyname/{myname}")]
        public async Task<IActionResult> getDepartmentbyname(string myname)
        {
            try
            {
                var result = await _mediator.Send(new GetDepartmentbynamerequest
                {
                    name = myname
                });
                _response.Result = result;
            }
            catch (Exception e)
            {
                _response.IsSuccess=false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return Ok(_response);
        }
    }
}
