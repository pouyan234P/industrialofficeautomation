using CorrespondenceCore.Api.DTO;
using CorrespondenceCore.Application.DTO;
using CorrespondenceCore.Application.Feature.AttachmentFeature.request.Commands;
using CorrespondenceCore.Application.Feature.AttachmentFeature.request.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CorrespondenceCore.Api.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class attachmentController : ControllerBase
    {
        protected ResponseDTO _response;
        private readonly IMediator _mediator;

        public attachmentController(IMediator mediator)
        {
            _mediator = mediator;
            this._response = new ResponseDTO();
        }

        [HttpPost("addAttachment")]
        public async Task<IActionResult> addAttachment([FromBody]setAttachmentDTO attachmentDTO)
        {
            try
            {
                var command = new createAttachmentCommand
                {
                    setAttachmentDTO = attachmentDTO
                };
                var response = await _mediator.Send(command);
                _response.Result = response;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages=new List<string>() { e.ToString() };
            }
            return Ok(_response);
        }

        [HttpGet("getAttachment/{id}")]
        public async Task<IActionResult> getAttachment(int id)
        {
            try
            {
                var response = await _mediator.Send(new getAttachmentDetailRequest
                {
                    id = id
                });
                _response.Result = response;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages=new List<string>() { e.ToString() } ;
            }
            return Ok(_response);
        }

        [HttpGet("getAttachments")]
        public async Task<IActionResult> getAttachments()
        {
            try
            {
                var response = await _mediator.Send(new getAttachmentRequest());
                _response.Result = response;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return Ok(_response);
        }

        [HttpPost("updateAttachment")]
        public async Task<IActionResult> updateAttachment([FromBody]AttachmentDTO dTO)
        {
            try
            {
                var command = new updateAttachmentCommand
                {
                    attachmentDTO = dTO
                };
                var response = await _mediator.Send(command);
                _response.Result = response;
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
