using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text.Json;
using System.Xml.Linq;
using webapi.Model;
using webapi.Model.Authentication;
using webapi.Services.IServices.Identity;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace webapi.Controllers.api.authentication
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class positionController : ControllerBase
    {
        private readonly IpositionIdentityService _service;
        private readonly IdepartmentIdentityService _service1;
        private readonly IDistributedCache _cache;
        // یک نام ثابت برای کلید کش در نظر می‌گیریم
        private const string POSITION_CACHE_KEY = "all_position_list";
        public positionController(IpositionIdentityService service,IdepartmentIdentityService service1, IDistributedCache cache)
        {
            _service = service;
            _service1 = service1;
            _cache = cache;
        }

        [HttpPost("insertPosition")]
        public async Task<IActionResult> insertPosition([FromBody] setPosition setPosition)
        {
           
            var response=await _service.insertPosition<ResponseDTO>(setPosition);
            if(response.IsSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.ErrorMessages);
        }

        [HttpGet("getPosition/{id}")]
        public async Task<IActionResult> getPosition(int id)
        {
            var response=await _service.getPosition<ResponseDTO>(id);
            if(response.IsSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.ErrorMessages);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> getAll()
        {
            // Adding this ensures that "Id" and "id" are treated the same way
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            try
            {
                // 1. Check the Cache
                var cachedDepartments = await _cache.GetStringAsync("POSITION_CACHE_KEY_V2"); // Changed key to bypass old bad data

                if (!string.IsNullOrEmpty(cachedDepartments))
                {
                    //return Content(cachedDepartments, "application/json");
                    // Cache HIT
                    var cachedData = JsonSerializer.Deserialize<IEnumerable<getPosition>>(cachedDepartments, jsonOptions);
                    return Ok(cachedData);
                }

                // 2. Fetch from Service (Cache MISS)
                var response = await _service.getAll<ResponseDTO>();

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
                    var depresult = JsonConvert.DeserializeObject<IEnumerable<getPosition>>(Convert.ToString(response.Result)!);
                    // 4. Save to Redis
                    string serializedData = JsonSerializer.Serialize(depresult, jsonOptions);
                    await _cache.SetStringAsync("POSITION_CACHE_KEY_V2", serializedData, cacheOptions);

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

        [HttpGet("getposanddept")]
        public async Task<IActionResult> getposanddept([FromBody]getposdep data)
        {
            var response=await _service.getposanddept<ResponseDTO>(data);
            if(response.IsSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response?.ErrorMessages);
        }

        [HttpGet("getPositionbyuser/{userid}")]
        public async Task<IActionResult> getPositionbyuser(int userid)
        {
            var response=await _service.getPositionbyuser<ResponseDTO>(userid);
            if(response.IsSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.ErrorMessages);
        }

        [HttpGet("getPositionbyDept/{deptid}")]
        public async Task<IActionResult> getPositionbyDept(int deptid)
        {
            var response=await _service.getPositionbyDept<ResponseDTO>(deptid);
            if( response.IsSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response!.ErrorMessages);
        }
    }
}
