
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi.Model;
using webapi.Model.Authentication;
using webapi.Services.IServices.Identity;

namespace webapi.Controllers.api.authentication
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class pictureController : ControllerBase
    {
        private readonly IpictureIdentityService _service;

        public pictureController(IpictureIdentityService service)
        {
            _service = service;
        }

        [HttpPost("addPicture")]
        public async Task<IActionResult> addPicture([FromForm]PictureUploadModel file)
        {
            var result = await _service.addPicture<ResponseDTO>(file.File);
            if(result.IsSuccess)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.ErrorMessages);
        }

        [HttpGet("getPicture/{id}")]
        public async Task<IActionResult> getPicture(int id)
        {
            var response = await _service.getPicture(id);

            if (response == null || response.ImageData == null)
                return NotFound("Image not found.");

            return File(response.ImageData, response.ContentType);
        }
    }
}
