using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using searchengine.Api.DTO;
using searchengine.Application.Feature.leatterFeature.request.Command;
using searchengine.Application.Feature.leatterFeature.request.Queries;
using searchengine.domain;

namespace searchengine.Api.Controllers.api
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class letterelsiController : ControllerBase
    {
        protected ResponseDTO _response;
        private readonly IMediator _mediator;

        public letterelsiController(IMediator mediator)
        {
            _mediator = mediator;
            _response= new ResponseDTO();
        }

        [HttpPost("addorupdate")]
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

        [HttpPost("search")]
        public async Task<IActionResult> search([FromBody] SearchRequestDto dto)
        {
            try
            {
                var result = await _mediator.Send(new SearchRequest
                {
                    keyword = dto.keyword,
                    fromDate = dto.fromDate,
                    toDate = dto.toDate
                });
                _response.Result = result;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages=new List<string>() { e.Message.ToString() };
            }
            return Ok(_response);
        }
    }
}
