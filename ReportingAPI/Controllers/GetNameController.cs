using Microsoft.AspNetCore.Authorization;
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
    [AllowAnonymous]
    public class GetNameController : ControllerBase
    {
        // GET: api/GetName
        [HttpGet]
        public async Task<string> GetUserName()
        {
            // base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated;
            //var tst = base.User.Identity.Name;
            //var tst2 = base.User.Identity.IsAuthenticated;

            return base.User.Identity.Name;
           // return BadRequest("tstadad");
        }
        [Authorize(Roles = @"EUROPE\KRR-LG_Inet_Users")]
        [HttpGet("{id}")]
        public async Task <List<string>> GetUser()
        {
            //List<string> User = new List<string>();
            var User = new List<string>();
            var tst = HttpContext.Request.Headers.ToArray();
            /*foreach(string t in tst)
            {

            }*/
            for(int i =0; i< tst.Length; i++)
            {
                User.Add(tst[i].ToString());
                //Console.WriteLine(tst[i]);
            }
            User.Add("Name: " + base.User.Identity.Name);

            
            //User.Add(HttpContext.User.Identities.ToString());
            //User.Add(HttpContext.User.Identity.Name);
            //User.Add(HttpContext.Request.Headers.ToList().ToArray<string>);
            return User;
        }
    }
}
