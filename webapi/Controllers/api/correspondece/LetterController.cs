using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using webapi.Model;
using webapi.Model.CorrespondenceModel;
using webapi.Services.IServices.ICorrespondenceService;

namespace webapi.Controllers.api.correspondece
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class LetterController : ControllerBase
    {
        private readonly ILetterCorrespondenceService _service;

        public LetterController(ILetterCorrespondenceService service)
        {
            _service = service;
        }

        [HttpPost("addLetter")]
        public async Task<IActionResult> addLetter([FromBody] setLetterDTO dto)
        {
            var response=await _service.addLetter<ResponseDTO>(dto);
            if(response.IsSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.ErrorMessages);
        }

        [HttpGet("getLetter/{id}")]
        public async Task<IActionResult> getLetter(int id)
        {
            var response=await _service.getLetter<ResponseDTO>(id);
            if(response.IsSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.ErrorMessages);
        }

        [HttpGet("getLetters")]
        public async Task<IActionResult> getLetters()
        {
            var response = await _service.getLetters<ResponseDTO>();
            if(response.IsSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.ErrorMessages);
        }

        [HttpGet("ping")]
        public async Task<IActionResult> Ping()
        {
            var response=await _service.Ping<ResponseDTO>();
            return Ok(response.Result);
        }
    }
}
