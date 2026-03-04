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
           // string error;
            /*var name = await _service.getDepartmentbyname<ResponseDTO>(dTO.Name!);
            if (name.IsSuccess)
            {
                // 1. Convert the generic object back to a JSON string
                string resultJson = JsonSerializer.Serialize(name.Result);

                // 2. Deserialize the string into your specific DTO
                // Note: PropertyNameCaseInsensitive is helpful so "ParentID" matches "parentId" from JSON
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                DepartmentDTO department = JsonSerializer.Deserialize<DepartmentDTO>(resultJson, options)!;
                dTO.ParentID = department.ParentID;
                var myresponse = await _service.addDepartment<ResponseDTO>(dTO);

                if (myresponse.IsSuccess)
                {
                    return Ok(myresponse.Result);
                }
                error = myresponse.ErrorMessages.FirstOrDefault()!;
                // Now you can use 'department.Id', 'department.Name', etc.
            }*/
          
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
