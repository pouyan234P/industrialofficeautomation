using authentication.api.DTO;
using authentication.application.DTO;
using authentication.application.Feature.positionRepository.request.Commands;
using authentication.application.Feature.positionRepository.request.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace authentication.api.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class positionController : ControllerBase
    {
        protected ResponseDTO _response;
        private readonly IMediator _mediator;

        public positionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("insertPosition")]
        public async Task<IActionResult> insertPosition([FromBody] setPosition setPosition)
        {
            try
            {
                var command = new createPositionCommand
                {
                    positionDTO = setPosition
                };
                var result = await _mediator.Send(command);
                _response.Result = result;
            }
            catch (Exception e)
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
    }
}

