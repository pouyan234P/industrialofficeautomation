using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using webapi.Model;
using webapi.Model.Searchengine;
using webapi.RabbmitmqSender;
using webapi.Services.IServices.ISearchEngineService;

namespace webapi.Controllers.api.search
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class letterelsiController : ControllerBase
    {
        private readonly IRabbitMQsearchMessageSender _sender;
        private readonly IConfiguration _configuration;
        private readonly IletterelsiSearchEngineService _service;

        public letterelsiController(IRabbitMQsearchMessageSender sender,IConfiguration configuration,IletterelsiSearchEngineService service)
        {
            _sender = sender;
            _configuration = configuration;
            _service = service;
        }

        [HttpPost("addorupdate")]
        public async Task<IActionResult> addorupdate([FromBody] LetterSearchDocument letterDocument)
        {
            _sender.SendMessage(letterDocument, _configuration.GetValue<string>("TopicAndQueueNames:myels")!);
            return await Task.FromResult(Ok());
        }

        [HttpPost("search")]
        public async Task<IActionResult> search([FromBody] SearchRequestDto dto)
        {
            var response=await _service.search<ResponseDTO>(dto);
            if(response.IsSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.ErrorMessages);
        }
    }
}
