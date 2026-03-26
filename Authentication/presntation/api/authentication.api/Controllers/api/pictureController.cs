using authentication.api.DTO;
using authentication.application.DTO;
using authentication.application.Feature.imagefeature.request.Commands;
using authentication.application.Feature.imagefeature.request.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace authentication.api.Controllers.api
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class pictureController : ControllerBase
    {
        private readonly IMediator _mediator;
        protected ResponseDTO _response;
        public pictureController(IMediator mediator)
        {
            _mediator = mediator;
            _response = new ResponseDTO();
        }

        [HttpPost("addPicture")]
        public async Task<IActionResult> addPicture([FromForm]IFormFile file)
        {
            
            try
            {
                if (file == null || file.Length == 0)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "No file was uploaded." };
                    return BadRequest(_response); // Return early if no file
                }

                // 2. Convert the IFormFile stream into a byte array
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();

                // 3. Populate your DTO
                var mysig = new signitureimageDTO
                {
                    ContentType = file.ContentType,
                    FileName = file.FileName,
                    ImageData = imageBytes // Pass the converted byte array here
                };
                var command = new createimageCommand
                {
                    signitureimageDTO = mysig
                };
                var response = await _mediator.Send(command);
                _response.Result = response;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages=new List<string>() { e.ToString()};
            }
            return Ok(_response);
        }

        [HttpGet("getPicture/{id}")]
        public async Task<IActionResult> getPicture(int id)
        {
            var result=await _mediator.Send(new getimageRequest
            {
                id = id
            });
            if (result == null)
            {
                return NotFound("Image not found.");
            }

            // 3. Return the byte array as a file
            // The File() method automatically sets the Content-Type header
            // so the browser knows how to render it (e.g., "image/jpeg")
            return File(result.ImageData, result.ContentType);
        }
    }
}
