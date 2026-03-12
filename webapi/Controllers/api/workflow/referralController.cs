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

        public referralController(IRabbitMQreferralMessageSender messageSender,IRabbitMQsearchMessageSender rabbitMQsearch,IConfiguration configuration,ILetterCorrespondenceService letterservice,IgenerateNextNumbeService numberservice)
        {
            _messageSender = messageSender;
            _rabbitMQsearch = rabbitMQsearch;
            _configuration = configuration;
            _letterservice = letterservice;
            _numberservice = numberservice;
        }

        [HttpPost("createreferral/{type}/{id}")]
        public async Task<IActionResult> createreferral([FromBody] setReferralDTO dto, TypeDTO type,int id)
        {
            var letter = await _letterservice.getLetter<ResponseDTO>(dto.LetterID);
            string jsonString = JsonConvert.SerializeObject(letter.Result);
            LetterDTO mydto = JsonConvert.DeserializeObject<LetterDTO>(jsonString)!;
            
            if (mydto.LetterNo == null)
            {
                var mygetdto = new getNextNumberDTO
                {
                    year = DateTime.Now.Year,
                    type = type,
                    deptID = id
                };
                var generateNumber = await _numberservice.mynextnumber<ResponseDTO>(mygetdto);
                string jsongenNumber=JsonConvert.SerializeObject(generateNumber.Result);
                string mynumber = JsonConvert.DeserializeObject<string>(jsongenNumber);
                dto.LetterNo =mynumber;
                mydto.LetterNo = mynumber;
                await _letterservice.updateLetter<ResponseDTO>(mydto);

            }
            else
                dto.LetterNo=mydto.LetterNo;
            var mysels = new LetterSearchDocument
            {
                LetterId=mydto.id,
                LetterNo=mydto.LetterNo!,
                LetterType=mydto.type.ToString(),
                Abstract=mydto.Abstract,
                CreatedDate=mydto.CreatedDate,
                PlainTextBody=mydto.BodyHTML,
                Subject=mydto.Subject,
                CreatorPositionId=dto.SenderPositionID,
                SenderDepartmentId=id
            };
            dto.LetterNo = mydto.LetterNo;
            dto.LetterSubject = mydto.Subject;
            dto.Timestamp=DateTime.Now;
            dto.Priority=Enum.Parse<priorityDTO>(mydto.priority.ToString());
            _messageSender.SendMessage(dto, _configuration.GetValue<string>("TopicAndQueueNames:myreferral"));
            _rabbitMQsearch.SendMessage(mysels, _configuration.GetValue<string>("TopicAndQueueNames:myels"));
            return Ok();
        }
    }
}
