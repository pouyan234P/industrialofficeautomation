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

        private const long MaxSignatureBytes = 2 * 1024 * 1024;

        /// <summary>Returns "image/png" or "image/jpeg" based on the file signature, or null.</summary>
        private static string? DetectImageType(byte[] data)
        {
            if (data.Length >= 8 && data[0] == 0x89 && data[1] == 0x50 && data[2] == 0x4E && data[3] == 0x47
                && data[4] == 0x0D && data[5] == 0x0A && data[6] == 0x1A && data[7] == 0x0A)
                return "image/png";
            if (data.Length >= 3 && data[0] == 0xFF && data[1] == 0xD8 && data[2] == 0xFF)
                return "image/jpeg";
            return null;
        }

        [RequestSizeLimit(MaxSignatureBytes + 64 * 1024)]
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

                if (file.Length > MaxSignatureBytes)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "The signature image must be 2 MB or smaller." };
                    return BadRequest(_response);
                }

                // 2. Convert the IFormFile stream into a byte array
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();

                // Trust the file's actual bytes, not the browser-supplied Content-Type,
                // so nothing but a real PNG/JPEG can be stored and later served back.
                var detectedType = DetectImageType(imageBytes);
                if (detectedType == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "Only PNG and JPEG images are allowed." };
                    return BadRequest(_response);
                }

                // 3. Populate your DTO
                var mysig = new signitureimageDTO
                {
                    ContentType = detectedType,
                    FileName = Path.GetFileName(file.FileName),
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
                _response.ErrorMessages=Errors.Unexpected(HttpContext, e);
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
