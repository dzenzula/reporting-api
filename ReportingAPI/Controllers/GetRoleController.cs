using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingApi.Controllers
{
    [SwaggerTag("Получить роль пользователя")]
    [Route("api/[controller]")]
    [ApiController]

    public class GetRoleController : ControllerBase
    {
        // GET: api/GetRole
        [HttpGet]
        public async Task<string> GetUserRole()
        {
            // base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated;
            //var tst = base.User.Identity.Name;
            //var tst2 = base.User.Identity.IsAuthenticated;
            string[] Admins = { 
                @"EUROPE\saarhipov", 
                @"EUROPE\fdeluca", 
                @"EUROPE\vlvshevchuk", 
                @"EUROPE\vgstotskiy", 
                @"EUROPE\dsguk", 
                @"EUROPE\evpavlovskaya"
            };
            
            string Username = User.Identity.Name;
            string Role = Admins.Contains(Username) ? "admin" : "guest";

            return Role;
            // return BadRequest("tstadad");
        }
    }
}
