using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi.Model;
using webapi.Model.CorrespondenceModel.Enum;
using webapi.Services.IServices.ICorrespondenceService;

namespace webapi.Controllers.api.correspondece
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndicatorController : ControllerBase
    {
        private readonly IgenerateNextNumbeService _service;

        public IndicatorController(IgenerateNextNumbeService service)
        {
            _service = service;
        }

        [HttpGet("getlastNumberbyType/{type}/{depid}/{year}")]
        public async Task<IActionResult> getlastNumberbyType(TypeDTO type,int depid,int year)
        {
            var response=await _service.getlastNumberbyType<ResponseDTO>(type,depid,year);
            if(response.IsSuccess)
            {
                return  Ok(response.Result);
            }
            return BadRequest(response.ErrorMessages);
        }
    }
}
