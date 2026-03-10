using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using webapi.Model.Searchengine;
using webapi.RabbmitmqSender;

namespace webapi.Controllers.api.search
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class letterelsiController : ControllerBase
    {
        private readonly IRabbitMQsearchMessageSender _sender;
        private readonly IConfiguration _configuration;

        public letterelsiController(IRabbitMQsearchMessageSender sender,IConfiguration configuration)
        {
            _sender = sender;
            _configuration = configuration;
        }

        [HttpPost("addorupdate")]
        public async Task<IActionResult> addorupdate([FromBody] LetterSearchDocument letterDocument)
        {
            _sender.SendMessage(letterDocument, _configuration.GetValue<string>("TopicAndQueueNames:myels"));
            return Ok();
        }
    }
}
