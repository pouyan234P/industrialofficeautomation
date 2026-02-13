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
        private readonly IMediator _mediator;

        public attachmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("addAttachment")]
        public async Task<IActionResult> addAttachment([FromBody]setAttachmentDTO attachmentDTO)
        {
            var command = new createAttachmentCommand
            {
                setAttachmentDTO = attachmentDTO
            };
            var respon=  await _mediator.Send(command);
            return Ok(respon);
        }

        [HttpGet("getAttachment/{id}")]
        public async Task<IActionResult> getAttachment(int id)
        {
            var response = await _mediator.Send(new getAttachmentDetailRequest
            {
                id = id
            });
            return Ok(response);
        }

        [HttpGet("getAttachments")]
        public async Task<IActionResult> getAttachments()
        {
            var response = await _mediator.Send(new getAttachmentRequest());
            return Ok(response);
        }

        [HttpPost("updateAttachment")]
        public async Task<IActionResult> updateAttachment([FromBody]AttachmentDTO dTO)
        {
            var command = new updateAttachmentCommand
            {
                attachmentDTO = dTO
            };
            var response=await _mediator.Send(command);
            return Ok(response);
        }
    }
}
