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
        public ReportsController(ReportingContext context)
        {
            _context = context;
        }

        // GET: api/WeightPlatforms
        [HttpGet]
        public async Task<List<Report>> GetReports()
        {
            return await _context.Reports.ToListAsync();
        }

    }
}
