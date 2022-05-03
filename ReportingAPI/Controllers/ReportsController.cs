using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportingApi.Dtos;
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
        public IMapper _mapper;
        public ReportsController(ReportingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/WeightPlatforms
        [HttpGet]
        public async Task<List<Report>> GetReports()
        {
            return await _context.Reports.ToListAsync();
        }

        // PUT: api/PutCategory
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<ActionResult> PutReport(UpdateReport report)
        {
            /*category.ParentId = null;
            category.Id = 7;
            category.Text = "test";*/ // Origin: test

            Report Reports = _mapper.Map<Report>(report);
            _context.Entry(Reports).State = EntityState.Modified;


            /*Console.WriteLine(category);
            return Ok();*/
            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Reports
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostReport(AddReport newReport)
        {
            Report report = _mapper.Map<Report>(newReport);
            try
            {
                _context.Reports.Add(report);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {

                return BadRequest(e.InnerException.Message);
            }

            return Ok(report.Id);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReport(int id)
        {
            var report = await _context.Reports.FindAsync(id);

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
