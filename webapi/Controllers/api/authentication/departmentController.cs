using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using webapi.Model;
using webapi.Model.Authentication;
using webapi.Services.IServices.Identity;

namespace webapi.Controllers.api.authentication
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class departmentController : ControllerBase
    {
        private readonly IdepartmentIdentityService _service;

        public departmentController(IdepartmentIdentityService service)
        {
            _service = service;
        }

        [HttpPost("addDepartment")]
        public async Task<IActionResult> addDepartment([FromBody] DepartmentDTO dTO)
        {
         
          
                var response = await _service.addDepartment<ResponseDTO>(dTO);
                if (response.IsSuccess)
                {
                    return Ok(response.Result);
                }
            return BadRequest(response.ErrorMessages);
        }

        [HttpGet("getDepartmentbyid/{id}")]
        public async Task<IActionResult> getDepartmentbyid(int id)
        {
            var response = await _service.getDepartmentbyid<ResponseDTO>(id);
            if (response.IsSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.ErrorMessages);
        }

        [HttpGet("getDepartment")]
        public async Task<IActionResult> getDepartment()
        {
            var response = await _service.getDepartment<ResponseDTO>();
            if (response.IsSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.ErrorMessages);
        }

        [HttpGet("getDepartmentbyname/{name}")]
        public async Task<IActionResult> getDepartmentbyname(string name)
        {
            var response=await _service.getDepartmentbyname<ResponseDTO>(name);
            if(response.IsSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.ErrorMessages);
        }
    }
}
