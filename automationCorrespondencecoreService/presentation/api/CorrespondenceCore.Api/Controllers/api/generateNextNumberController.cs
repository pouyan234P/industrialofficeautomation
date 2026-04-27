using CorrespondenceCore.Api.DTO;
using CorrespondenceCore.Application.DTO.Enum;
using CorrespondenceCore.Application.Feature.GenerateNextNumberAsyncFeature.request;
using CorrespondenceCore.Application.Feature.GenerateNextNumberAsyncFeature.request.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CorrespondenceCore.Api.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class generateNextNumberController : ControllerBase
    {
        private readonly IMediator _mediator;
        protected ResponseDTO _response;
        public generateNextNumberController(IMediator mediator)
        {
            _mediator = mediator;
            _response=new ResponseDTO();
        }

        [HttpGet("getnextnumber")]
        public async Task<IActionResult> getnextnumber(getNextNumberDTO dto)
        {
            var result=await _mediator.Send(new generateNextNumberRequest
            {
                depid=(int)dto.deptID!,
                type=dto.type,
                year=dto.year
            });
            _response.Result = result;
            return Ok(_response);

        }

        [HttpGet("getlastNumberbyType/{type}/{depid}/{year}")]
        public async Task<IActionResult> getlastNumberbyType(TypeDTO type,int depid,int year)
        {
            var result = await _mediator.Send(new getlastNumberbyTypeRequest
            {
                type = type,
                depid = depid,
                year = year
            });
            _response.Result = result;
            return Ok(_response);
        }
    }
}
