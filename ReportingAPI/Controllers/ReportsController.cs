using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportingAPI.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingAPI.Controllers
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
        public async Task<int> GetWeightPlatforms()
        {
            /*if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                return await _context.WeightPlatforms.ToListAsync();
            }
            else
            {*/
                return 0;
            /*}*/
        }

    }
}
