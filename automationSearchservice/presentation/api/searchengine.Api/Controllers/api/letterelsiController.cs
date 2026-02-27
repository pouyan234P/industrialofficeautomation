using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using searchengine.Api.DTO;
using searchengine.Application.Feature.leatterFeature.request.Command;
using searchengine.Application.Feature.leatterFeature.request.Queries;
using searchengine.domain;

namespace searchengine.Api.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class letterelsiController : ControllerBase
    {
        protected ResponseDTO _responseDTO;
        private readonly IMediator _mediator;

        public letterelsiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("search")]
        public async Task<IActionResult> addorupdate([FromBody] LetterSearchDocument letterDocument)
        {

            // اینجا باید DTO یا مدل مناسبی تعریف کنید که شامل اطلاعات نامه باشد
            // فرض می‌کنیم یک LetterSearchDocument داریم که شامل اطلاعات لازم است
            var command = new createandupdateLetterCommand
            {
                document = letterDocument // این را به مدل مناسب تبدیل کنید
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> search([FromBody] string keyword, [FromBody] DateTime? fromDate, [FromBody] DateTime? toDate)
        {
            var result = await _mediator.Send(new SearchRequest
            {
                keyword = keyword,
                fromDate = fromDate,
                toDate = toDate
            });
            return Ok(result);
        }
    }
}
