using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared.Infrastructure.Middleware;
using webapi.Model;
using webapi.Model.CorrespondenceModel;
using webapi.Model.CorrespondenceModel.Enum;
using webapi.Model.Searchengine;
using webapi.Model.Workflow;
using webapi.Model.Workflow.Enum;
using webapi.RabbmitmqSender;
using webapi.Services.IServices.ICorrespondenceService;
using webapi.Services.IServices.IWorkflowService;

namespace webapi.Controllers.api.workflow
{
    [AllowAnonymous]   // TODO: replace with [Authorize] once Identity service is ready
    [Route("api/[controller]")]
    [ApiController]
    public class referralController : ControllerBase
    {
        private readonly IRabbitMQreferralMessageSender _messageSender;
        private readonly IRabbitMQsearchMessageSender _rabbitMQsearch;
        private readonly IConfiguration _configuration;
        private readonly ILetterCorrespondenceService _letterservice;
        private readonly IgenerateNextNumbeService _numberservice;
        private readonly IreferralWorkflowService _referralservice;
        private readonly ILogger<referralController> _logger;

        public referralController(
            IRabbitMQreferralMessageSender messageSender,
            IRabbitMQsearchMessageSender rabbitMQsearch,
            IConfiguration configuration,
            ILetterCorrespondenceService letterservice,
            IgenerateNextNumbeService numberservice,
            IreferralWorkflowService referralservice,
            ILogger<referralController> logger)
        {
            _messageSender = messageSender;
            _rabbitMQsearch = rabbitMQsearch;
            _configuration = configuration;
            _letterservice = letterservice;
            _numberservice = numberservice;
            _referralservice = referralservice;
            _logger = logger;
        }

        [HttpPost("createreferral/{id}")]
        public async Task<IActionResult> createreferral([FromBody] setReferralDTO dto, int id)
        {
            // Pull the CorrelationId injected by CorrelationIdMiddleware
            // This is the same ID that will flow into both RabbitMQ messages
            // so you can trace the full create-referral journey across all services
            var correlationId = HttpContext.Items[CorrelationIdMiddleware.HeaderName]?.ToString()
                                ?? Guid.NewGuid().ToString();

            _logger.LogInformation(
                "CreateReferral started. LetterId: {LetterId}, DepartmentId: {DeptId}, CorrelationId: {CorrelationId}",
                dto.LetterID, id, correlationId);

            var letter = await _letterservice.getLetter<ResponseDTO>(dto.LetterID);
            string jsonString = JsonConvert.SerializeObject(letter.Result);
            LetterDTO mydto = JsonConvert.DeserializeObject<LetterDTO>(jsonString)!;

            if (mydto.LetterNo == null)
            {
                _logger.LogInformation(
                    "Letter {LetterId} is a draft — generating number. CorrelationId: {CorrelationId}",
                    dto.LetterID, correlationId);

                var mygetdto = new getNextNumberDTO
                {
                    year = DateTime.Now.Year,
                    type = dto.type,
                    deptID = id
                };

                var generateNumber = await _numberservice.mynextnumber<ResponseDTO>(mygetdto);
                string jsongenNumber = JsonConvert.SerializeObject(generateNumber.Result);
                string mynumber = JsonConvert.DeserializeObject<string>(jsongenNumber)!;

                dto.LetterNo = mynumber;
                mydto.LetterNo = mynumber;
                mydto.SentDate = DateTime.Now;

                await _letterservice.updateLetter<ResponseDTO>(mydto);

                _logger.LogInformation(
                    "Letter {LetterId} assigned number {LetterNo}. CorrelationId: {CorrelationId}",
                    dto.LetterID, mynumber, correlationId);

                // Publish to search index — forward correlationId so search consumer logs match
                var searchDoc = new LetterSearchDocument
                {
                    LetterId = mydto.id,
                    LetterNo = mydto.LetterNo!,
                    LetterType = mydto.type.ToString(),
                    Abstract = mydto.Abstract,
                    CreatedDate = mydto.CreatedDate,
                    PlainTextBody = mydto.BodyHTML,
                    Subject = mydto.Subject,
                    CreatorPositionId = dto.SenderPositionID,
                    SenderDepartmentId = id
                };

               await _rabbitMQsearch.SendMessage(
                    searchDoc,
                    _configuration.GetValue<string>("TopicAndQueueNames:myels")!,
                    correlationId);  // ← CorrelationId flows into search consumer logs
            }
            else
            {
                dto.LetterNo = mydto.LetterNo;
            }

            dto.LetterNo = mydto.LetterNo;
            dto.LetterSubject = mydto.Subject;
            dto.Timestamp = DateTime.Now;
            dto.priority = Enum.Parse<priorityDTO>(mydto.priority.ToString());

            // Publish referral — forward correlationId so workflow consumer logs match
            await  _messageSender.SendMessage(
                dto,
                _configuration.GetValue<string>("TopicAndQueueNames:myreferral")!,
                correlationId);  // ← CorrelationId flows into workflow consumer logs

            _logger.LogInformation(
                "CreateReferral completed. LetterId: {LetterId}, CorrelationId: {CorrelationId}",
                dto.LetterID, correlationId);

            return Ok();
        }

        [HttpGet("getAllByPositon/{id}")]
        public async Task<IActionResult> getAllByPositon(int id)
        {
            _logger.LogInformation("GetAllByPosition called. PositionId: {PositionId}", id);
            var response = await _referralservice.getAllByPositon<ResponseDTO>(id);
            if (response.IsSuccess) return Ok(response.Result);
            return BadRequest(response.ErrorMessages);
        }

        [HttpGet("getAllByReciver/{id}")]
        public async Task<IActionResult> getAllByReciver(int id)
        {
            _logger.LogInformation("GetAllByReceiver called. PositionId: {PositionId}", id);
            var response = await _referralservice.getAllByReciver<ResponseDTO>(id);
            if (response.IsSuccess) return Ok(response.Result);
            return BadRequest(response.ErrorMessages);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> getAll()
        {
            var response = await _referralservice.getAll<ResponseDTO>();
            if (response.IsSuccess) return Ok(response.Result);
            return BadRequest(response.ErrorMessages);
        }

        [HttpPost("updatereferral")]
        public async Task<IActionResult> updatereferral([FromBody] referralDTO dTO)
        {
            var correlationId = HttpContext.Items[CorrelationIdMiddleware.HeaderName]?.ToString()
                                ?? Guid.NewGuid().ToString();

            _logger.LogInformation(
                "UpdateReferral called. ReferralId: {ReferralId}, CorrelationId: {CorrelationId}",
                dTO.id, correlationId);

            _messageSender.UpdateMessage(
                dTO,
                _configuration.GetValue<string>("TopicAndQueueNames:updatereferral")!,
                correlationId);

            return await Task.FromResult(Ok());
        }
    }
}