using CorrespondenceCore.Api.DTO;
using CorrespondenceCore.Application.DTO;
using CorrespondenceCore.Application.Feature.htmlbodyFeature.request.Commands;
using CorrespondenceCore.Application.Feature.htmlbodyFeature.request.Queries;
using CorrespondenceCore.Application.Feature.LetterFeature.request.Command;
using CorrespondenceCore.Application.Feature.LetterFeature.request.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace CorrespondenceCore.Api.Controllers.api
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class LetterController : ControllerBase
    {
        protected ResponseDTO _response;
        private readonly IMediator _mediator;

        public LetterController(IMediator mediator)
        {
            _mediator = mediator;
            this._response = new ResponseDTO();
        }

        [HttpPost("addLetter")]
        public async Task<IActionResult> addLetter([FromBody]setLetterDTO letterDTO)
        {
            try
            {
                var bsonDocument = BsonDocument.Parse(letterDTO.BodyHTML!.ToString());
                var commandhtml = new createhtmlbodyCommand
                {
                    elements = bsonDocument
                };
                var result = await _mediator.Send(commandhtml);
                letterDTO.BodyHTML = result["_id"]!.ToString();
                var command = new createLetterCommand
                {
                    setLetterDTO = letterDTO
                };
                _response.Result = await _mediator.Send(command);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages= new List<string>() { e.ToString()};    
            }

            return Ok(_response);
        }

        [HttpGet("getLetter/{id}")]
        public async Task<IActionResult> getLetter(int id)
        {
            try
            {
                var response = await _mediator.Send(new getLetterDetailRequest
                {
                    id = id
                });
                var result = await _mediator.Send(new gethtmlbodyRequest
                {
                    id = response.BodyHTML.ToString()
                });
                response.BodyHTML = result["context"].ToString();
                _response.Result = response;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages=new List<string>() { e.ToString()};
            }
            return Ok(_response);
        }

        [HttpGet("getLetters")]
        public async Task<IActionResult> getLetters()
        {
            try
            {
                var response = await _mediator.Send(new getLetterRequest());
                _response.Result = response;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages=new List<string> { e.ToString()};
            }
            return Ok(_response);
        }

        [HttpPost("updateLetter")]
        public async Task<IActionResult> updateLetter([FromBody]LetterDTO myletterDTO)
        {
            try
            {
                var command = new updateLetterCommand
                {
                    letterDTO = myletterDTO
                };
                var response = await _mediator.Send(command);
                _response.Result = response;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages=new List<string>() { e.ToString()};
            }
            return Ok(_response);
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            _response.Result = "Pong from Letter Service!";
            return Ok(_response);
        }

        /* public async Task<IActionResult> deleteLetter(int id)
         {

         }*/
    }
}
