using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using System.Security.Principal;
using System.Text.Json.Serialization;
using ScalesMWebAPI.Services.Handlers;
using ScalesMWebAPI.Dtos;
using System;

namespace ScalesMWebAPI.Controllers
{
    [SwaggerTag("Возвращает имя пользователя")]
    [Route("api/[controller]")]
    [ApiController]
    public class GetNameController : ControllerBase
    {
        public GetNameController() 
        {
        
        }
        [AllowAnonymous]
        [Consumes(MediaTypeNames.Application.Json)]
        [HttpGet]
        public ActionResult<GetUserNameDto> GetUserName()
        {
            GetUserNameDto user = new GetUserNameDto();
            string user_name = "";
            string user_domain = "";

            Security.GetUserNameDomain(HttpContext.User, out user_name, out user_domain);
            user.user = user_name;
            user.domain = user_domain;
            user.client_host = HttpContext.Connection.RemoteIpAddress.ToString();
            user.time = DateTime.Now;
            //user_name = "[ { \"" + "user" + "\"" + ":" + "\"" + user_name + "\"" + ", " + "\"domain\"" + ": \"" + user_domain + "\"" + " } ]";
            //base.Response.ContentLength = user_name.Length;
            return user;
        }
    }

    //public class User 
    //{
    //    private string user_name;
    //    private string user_domain;
    //    [JsonInclude]
    //    public string name { get => user_name; set => user_name = value; }
    //    [JsonInclude]
    //    public string domain { get => user_domain; set => user_domain = value; }
    //    public User(IIdentity identity) 
    //    {
    //        if (identity.Name != null)
    //        {
    //            int hasDomain = identity.Name.IndexOf(@"\");
    //            if (hasDomain > 0)
    //            {
    //                user_domain = identity.Name.Substring(0, hasDomain);
    //                user_name = identity.Name.Remove(0, hasDomain + 1);
    //            }
    //        }
    //    }
    //    public override string ToString()
    //    {
    //        return "[ { \"" + "user" + "\"" + ":" + "\"" + user_name + "\"" + ", " + "\"domain\"" + ": \"" + user_domain + "\"" + " } ]"; ;
    //    }
    //}
}
