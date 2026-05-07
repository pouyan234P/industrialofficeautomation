using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using workflow.Api.DTO;
using workflow.Application.DTO.Enum;
using workflow.Application.helper;
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
            this._response = new ResponseDTO();
        }

        [HttpPost("createreferral")]
        public async Task<IActionResult> createreferral([FromBody] setReferralDTO setReferralDTO)
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
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
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
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return Ok(_response);
        }

        [HttpGet("getAllByReciver/{id}")]
        public async Task<IActionResult> getAllByReciver(int id, [FromQuery] UserParams userParams)
        {
            try
            {
                string myid = id.ToString();
                var result = await _mediator.Send(new getReferralbyreciverRequest
                {
                    id = myid,
                    userParams = userParams
                });
                _response.Result = result;
                _response.currentPage = result.CurrentPage;
                _response.itemsPerPage = result.PageSize;
                _response.totalItems = result.TotalCount;
                _response.totalPages = result.TotalPage;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
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
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
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
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return Ok(_response);
        }

        [HttpPost("getbytyperecvierid/{receverid}")]
        public async Task<IActionResult> getbytyperecvierid(int receverid, [FromBody] TypeDTO type, [FromQuery] UserParams userParams)
        {
            try
            {
                string id = Convert.ToString(receverid);
                var result = await _mediator.Send(new getReferralbyTypeRequest
                {
                    mytype = type,
                    reciverID = id,
                    UserParams=userParams
                });
                _response.Result = result;
                _response.currentPage = result.CurrentPage;
                _response.itemsPerPage = result.PageSize;
                _response.totalItems = result.TotalCount;
                _response.totalPages = result.TotalPage;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return Ok(_response);
        }

        [HttpGet("getAllbySenderposition/{id}")]
        public async Task<IActionResult> getAllbySenderposition(int id, [FromQuery] UserParams userParams)
        {
            try
            {
                string myid = Convert.ToString(id);
                var result = await _mediator.Send(new getAllbySenderpositionRequest
                {
                    id = myid,
                    userparams=userParams
                });
                _response.Result = result;
                _response.currentPage = result.CurrentPage;
                _response.itemsPerPage = result.PageSize;
                _response.totalItems = result.TotalCount;
                _response.totalPages = result.TotalPage;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return Ok(_response);
        }

        [HttpPost("getReferralbyTypeSenderid/{senderid}")]
        public async Task<IActionResult> getReferralbyTypeSenderid(int senderid, [FromBody] TypeDTO type, [FromQuery] UserParams userParams)
        {
            try
            {
                string id = Convert.ToString(senderid);
                var result = await _mediator.Send(new getReferralbyTypeSenderidRequest
                {
                    mytype = type,
                    senderid = id,
                    userParams=userParams
                });
                _response.Result = result;
                _response.currentPage = result.CurrentPage;
                _response.itemsPerPage = result.PageSize;
                _response.totalItems = result.TotalCount;
                _response.totalPages = result.TotalPage;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return Ok(_response);
        }
    }
}
