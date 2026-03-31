using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text.Json;
using webapi.Model;
using webapi.Model.Authentication;
using webapi.Services.IServices.Identity;
using JsonException = Newtonsoft.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace webapi.Controllers.api.authentication
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class departmentController : ControllerBase
    {
        private readonly IdepartmentIdentityService _service;
        private readonly IDistributedCache _cache;
        // یک نام ثابت برای کلید کش در نظر می‌گیریم
        private const string DEPARTMENTS_CACHE_KEY = "all_departments_list";
        public departmentController(IdepartmentIdentityService service, IDistributedCache cache)
        {
            _service = service;
            _cache = cache;
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
        public async Task<IActionResult> GetDepartment()
        {
            // Adding this ensures that "Id" and "id" are treated the same way
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            try
            {
                // 1. Check the Cache
                var cachedDepartments = await _cache.GetStringAsync("DEPARTMENTS_CACHE_KEY_V2"); // Changed key to bypass old bad data

                if (!string.IsNullOrEmpty(cachedDepartments))
                {
                    //return Content(cachedDepartments, "application/json");
                    // Cache HIT
                    var cachedData = JsonSerializer.Deserialize<IEnumerable<DepartmentDTO>>(cachedDepartments, jsonOptions);
                    return Ok(cachedData);
                }

                // 2. Fetch from Service (Cache MISS)
                var response = await _service.getDepartment<ResponseDTO>();

                if (response != null && response.IsSuccess)
                {
                    if (response.Result == null)
                    {
                        return NotFound("هیچ دپارتمانی در سیستم ثبت نشده است.");
                    }

                    // 3. Set Cache Expiration
                    var cacheOptions = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
                    };
                  var depresult=  JsonConvert.DeserializeObject<IEnumerable<DepartmentDTO>>(Convert.ToString(response.Result)!);
                    // 4. Save to Redis
                    string serializedData = JsonSerializer.Serialize(depresult, jsonOptions);
                    await _cache.SetStringAsync("DEPARTMENTS_CACHE_KEY_V2", serializedData, cacheOptions);

                    return Ok(response.Result);
                }

                // Return the error from the service
                return BadRequest(response?.ErrorMessages);
            }
            catch (JsonException jsonEx)
            {
                // If the cache somehow gets corrupted again, we catch it here so the app doesn't crash
                return StatusCode(500, $"خطای پردازش داده‌های کش: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"خطای داخلی سرور: {ex.Message}");
            }
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
