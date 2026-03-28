using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using webapi.Model;
using webapi.Model.Authentication;
using webapi.Services.IServices.Identity;

namespace webapi.Controllers.api.authentication
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class userController : ControllerBase
    {
        private readonly IauthIdentityService _service;
        private readonly IpositionIdentityService _service1;

        public userController(IauthIdentityService service,IpositionIdentityService service1)
        {
            _service = service;
            _service1 = service1;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> getAll()
        {
            var usermodel=new List<userModel>();
            var response = await _service.getAll<ResponseDTO>();
            if (response.IsSuccess)
            {
                var usersList = JsonConvert.DeserializeObject<IEnumerable<getuserDTO>>(Convert.ToString(response.Result)!);

                if (usersList != null)
                {
                    foreach (var i in usersList)
                    {
                        string uploaded;
                        if(i.signitureimageid != null)
                        {
                            uploaded = "uploaded";
                        }
                        else
                        {
                            uploaded = "not uploaded";
                        }
                        var getpos = await _service1.getPosition<ResponseDTO>(i.id);
                        if (getpos.IsSuccess)
                        {
                            var mygetpos = JsonConvert.DeserializeObject<getPosition>(Convert.ToString(getpos.Result)!);
                            var myuser = new userModel
                            {
                                Title = mygetpos.Title,
                                CurrentStatus = i.CurrentStatus,
                                username = i.username,
                                signitureimageid = uploaded
                            };
                            usermodel.Add(myuser);
                        }
                        
                    }
                }
                return Ok(usermodel);
            }
            return BadRequest(response.ErrorMessages);
        }
    }
}
