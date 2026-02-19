using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using workflow.Appliction.DTO;
using workflow.Appliction.Feature.referralFeature.request.Command;
using workflow.Appliction.Feature.referralFeature.request.Queries;

namespace workflow.Api.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class referralController : ControllerBase
    {
        private readonly IMediator _mediator;

        public referralController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("createreferral")]
        public async Task<IActionResult> createreferral([FromBody]setReferralDTO setReferralDTO)
        {
            var command = new createReferralCommand
            {
                setReferral = setReferralDTO
            };
            var result=await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("getAllByPositon/{id}")]
        public async Task<IActionResult> getAllByPositon(int id)
        {
            string myid=id.ToString();
            var result = await _mediator.Send(new getReferralbyPositionRequest
            {
                SenderPositionID=myid
            });
            return Ok(result);
        }

        [HttpGet("getreferral/{id}")]
        public async Task<IActionResult> getreferral(int id)
        {
            string myid=id.ToString();
            var result = await _mediator.Send(new getReferraldetailRequest
            {
                id=myid
            });
            return Ok(result);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> getAll()
        {
            var result = await _mediator.Send(new getReferralRequest());
            return Ok(result);
        }
    }
}
