using authentication.api.DTO;
using authentication.application.DTO;
using authentication.application.Feature.positionRepository.request.Commands;
using authentication.application.Feature.positionRepository.request.Queries;
using authentication.domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace authentication.api.Controllers.api
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class positionController : ControllerBase
    {
        protected ResponseDTO _response;
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManger;

        public positionController(IMediator mediator, UserManager<User> userManger)
        {
            _mediator = mediator;
            _userManger = userManger;
            _response = new ResponseDTO();
        }

        [HttpPost("insertPosition")]
        public async Task<IActionResult> insertPosition([FromBody] setPosition myPosition)
        {
            try
            {
               
                var command = new createPositionCommand
                {
                    positionDTO = myPosition
                };
                var result = await _mediator.Send(command);
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

        [HttpGet("getPosition/{id}")]
        public async Task<IActionResult> getPosition(int id)
        {
            try
            {
                var result = await _mediator.Send(new getpositionrequest
                {
                    id = id
                });
                _response.Result = result;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
                
            }
            return Ok(_response);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> getAll()
        {
            try
            {
                var result = await _mediator.Send(new getPositionsRequest());
                _response.Result = result;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return Ok(_response);
        }

        [HttpGet("getposanddept")]
        public async Task<IActionResult> getposanddept([FromBody]getposdep pos)
        {
            try
            {
                var result = await _mediator.Send(new getPositonbydepandposRequest
                {
                    depid = pos.depid,
                    posid=pos.posid
                });
                _response.Result = result;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages= new List<string>() { e.ToString() } ;
            }
            return Ok(_response);
        }

        [HttpGet("getPositionbyuser/{userid}")]
        public async Task<IActionResult> getPositionbyuser(int userid)
        {
            try
            {
                var result = await _mediator.Send(new getPositionbyuserRequest
                {
                    userid=userid,
                });
                _response.Result = result;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages=new List<string> { e.ToString() } ;
            }
            return Ok(_response);
        }

        [HttpGet("getPositionbyDept/{deptid}")]
        public async Task<IActionResult> getPositionbyDept(int deptid)
        {
            try
            {
                var result = await _mediator.Send(new getPositionbyDeptRequest
                {
                    deptid = deptid,
                });
                _response.Result = result;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return Ok(_response);
        }
    }
}

