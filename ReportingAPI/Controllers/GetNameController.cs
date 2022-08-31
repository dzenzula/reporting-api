using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingApi.Controllers
{
    [SwaggerTag("Получить имя пользователя")]
    [Route("api/[controller]")]
    [ApiController]
    //[AllowAnonymous]
    public class GetNameController : ControllerBase
    {
        // GET: api/GetName
        [HttpGet]
        public string GetUserName()
        {
            return base.User.Identity.Name;
        }

        [HttpGet("{id}")]
        public List<string> GetUser()
        {
            var User = new List<string>();
            var tst = HttpContext.Request.Headers.ToArray();

            for (int i = 0; i < tst.Length; i++)
            {
                User.Add(tst[i].ToString());
            }
            User.Add("Name: " + base.User.Identity.Name);


            //User.Add(HttpContext.User.Identities.ToString());
            //User.Add(HttpContext.User.Identity.Name);
            //User.Add(HttpContext.Request.Headers.ToList().ToArray<string>);
            return User;
        }
    }
}
