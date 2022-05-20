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
        // GET: api/WeightPlatforms
        [HttpGet]
        public async Task<string> GetUsername()
        {
           // base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated;
            //var tst = base.User.Identity.Name;
            //var tst2 = base.User.Identity.IsAuthenticated;
            return base.User.Identity.Name;
        }
        
        
        /*private readonly ReportingContext _context;
        public IMapper _mapper;
        public ReportsController(ReportingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }*/

        // GET: api/WeightPlatforms
        /*[HttpGet]
        public async Task<List<Report>> GetReports()
        {
            return await _context.Reports.ToListAsync();
        }*/
    }
}
