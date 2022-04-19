using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportingApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingApi.Controllers
{
    [SwaggerTag("Отчеты")]
    [Route("api/[controller]")]
    [ApiController]

    public class ReportsController : ControllerBase
    {
        private readonly ReportingContext _context;
        private IMapper _mapper;
        public ReportsController(ReportingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/WeightPlatforms
        [HttpGet]
        public async Task<List<Report>> GetReports()
        {
            /*if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                return await _context.WeightPlatforms.ToListAsync();
            }
            else
            {*/
                return await _context.Reports.ToListAsync(); 
            /*}*/
        }

    }
}
