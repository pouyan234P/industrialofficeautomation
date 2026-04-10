using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
    [AllowAnonymous]
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

        public referralController(IRabbitMQreferralMessageSender messageSender,IRabbitMQsearchMessageSender rabbitMQsearch,IConfiguration configuration,ILetterCorrespondenceService letterservice,IgenerateNextNumbeService numberservice,IreferralWorkflowService referralservice)
        {
            _messageSender = messageSender;
            _rabbitMQsearch = rabbitMQsearch;
            _configuration = configuration;
            _letterservice = letterservice;
            _numberservice = numberservice;
            _referralservice = referralservice;
        }

        [HttpPost("createreferral/{id}")]
        public async Task<IActionResult> createreferral([FromBody] setReferralDTO dto, int id)
        {
            var letter = await _letterservice.getLetter<ResponseDTO>(dto.LetterID);
            string jsonString = JsonConvert.SerializeObject(letter.Result);
            LetterDTO mydto = JsonConvert.DeserializeObject<LetterDTO>(jsonString)!;

            if (mydto.LetterNo == null)
            {
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
                mydto.LetterNo = mynumber!;
                mydto.SentDate = DateTime.Now;
                await _letterservice.updateLetter<ResponseDTO>(mydto);
                var mysels = new LetterSearchDocument
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
               

                _rabbitMQsearch.SendMessage(mysels, _configuration.GetValue<string>("TopicAndQueueNames:myels")!);
            }
            else
                dto.LetterNo = mydto.LetterNo;

            dto.LetterNo = mydto.LetterNo;
            dto.LetterSubject = mydto.Subject;
            dto.Timestamp = DateTime.Now;
            dto.priority = Enum.Parse<priorityDTO>(mydto.priority.ToString());


            _messageSender.SendMessage(dto, _configuration.GetValue<string>("TopicAndQueueNames:myreferral")!);

            return Ok();
        }

        [HttpGet("getAllByPositon/{id}")]
        public async Task<IActionResult> getAllByPositon(int id)
        {
            var response = await _referralservice.getAllByPositon<ResponseDTO>(id);
            if(response.IsSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.ErrorMessages);
        }

        [HttpGet("getAllByReciver/{id}")]
        public async Task<IActionResult> getAllByReciver(int id)
        {
            var response = await _referralservice.getAllByReciver<ResponseDTO>(id);
            if (response.IsSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.ErrorMessages);
        }
        [HttpGet("getAll")]
        public async Task<IActionResult> getAll()
        {
            var response = await _referralservice.getAll<ResponseDTO>();
            if(response.IsSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.ErrorMessages);
        }

        [HttpPost("updatereferral")]
        public async Task<IActionResult> updatereferral([FromBody] referralDTO dTO)
        { 

            _messageSender.UpdateMessage(dTO, _configuration.GetValue<string>("TopicAndQueueNames:updatereferral"));
            return await Task.FromResult(Ok());
        }

        [HttpPost("getbytyperecvierid/{reciveid}")]
        public async Task<IActionResult> getbytyperecvierid(int reciveid,[FromBody]TypeDTO type)
        {
            var response = await _referralservice.getbytyperecvierid<ResponseDTO>(reciveid, type);
            if (response.IsSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.ErrorMessages);
        }
    }
}
