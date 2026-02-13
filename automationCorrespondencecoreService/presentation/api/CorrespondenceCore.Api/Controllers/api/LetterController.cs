using CorrespondenceCore.Application.DTO;
using CorrespondenceCore.Application.Feature.LetterFeature.request.Command;
using CorrespondenceCore.Application.Feature.LetterFeature.request.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CorrespondenceCore.Api.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class LetterController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LetterController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("addLetter")]
        public async Task<IActionResult> addLetter([FromBody]setLetterDTO letterDTO)
        {
            var command = new createLetterCommand
            {
                setLetterDTO = letterDTO
            };
            var response=await _mediator.Send(command);
            return Ok(response);
        }

        [HttpGet("getLetter/{id}")]
        public async Task<IActionResult> getLetter(int id)
        {
            var response = await _mediator.Send(new getLetterDetailRequest
            {
                id = id
            });
            return Ok(response);
        }

        [HttpGet("getLetters")]
        public async Task<IActionResult> getLetters()
        {
            var response = await _mediator.Send(new getLetterRequest());
            return Ok(response);
        }

        [HttpPost("updateLetter")]
        public async Task<IActionResult> updateLetter([FromBody]LetterDTO myletterDTO)
        {
            var command = new updateLetterCommand
            {
                letterDTO = myletterDTO
            };
            var response= await _mediator.Send(command);
            return Ok(response);
        }

       /* public async Task<IActionResult> deleteLetter(int id)
        {

        }*/
    }
}
