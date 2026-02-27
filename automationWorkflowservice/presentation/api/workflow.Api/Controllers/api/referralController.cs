using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using workflow.Api.DTO;
using workflow.Appliction.DTO;
using workflow.Appliction.Feature.referralFeature.request.Command;
using workflow.Appliction.Feature.referralFeature.request.Queries;

namespace workflow.Api.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class referralController : ControllerBase
    {
        protected ResponseDTO _response;
        private readonly IMediator _mediator;

        public referralController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("createreferral")]
        public async Task<IActionResult> createreferral([FromBody]setReferralDTO setReferralDTO)
        {
            try
            {
                var command = new createReferralCommand
                {
                    setReferral = setReferralDTO
                };
                var result = await _mediator.Send(command);
                _response.Result = result;
            }
            catch (Exception e)
            {
                _response.IsSuccess=false;
                _response.ErrorMessages=new List<string>() { e.ToString() };
            }
            return Ok(_response);
        }

        [HttpGet("getAllByPositon/{id}")]
        public async Task<IActionResult> getAllByPositon(int id)
        {
            try
            {
                string myid = id.ToString();
                var result = await _mediator.Send(new getReferralbyPositionRequest
                {
                    SenderPositionID = myid
                });
                _response.Result = result;
            }
            catch (Exception e)
            {
                _response.IsSuccess=false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return Ok(_response);
        }

        [HttpGet("getreferral/{id}")]
        public async Task<IActionResult> getreferral(int id)
        {
            try
            {
                string myid = id.ToString();
                var result = await _mediator.Send(new getReferraldetailRequest
                {
                    id = myid
                });
                _response.Result = result;
            }
            catch (Exception e)
            {
                _response.IsSuccess=false;
                _response.ErrorMessages= new List<string>() { e.ToString() } ;
            }
            return Ok(_response);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> getAll()
        {
            try
            {
                var result = await _mediator.Send(new getReferralRequest());
                _response.Result = result;
            }
            catch (Exception e)
            {
                _response.IsSuccess=false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return Ok(_response);
        }
    }
}
