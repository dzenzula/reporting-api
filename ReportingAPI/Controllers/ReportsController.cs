using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportingApi.Dtos;
using ReportingApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingApi.Controllers
{
    [SwaggerTag("Отчеты")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = @"EUROPE\KRR-LG_Inet_Users")]
    public class ReportsController : ControllerBase
    {
        private readonly ReportingContext _context;
        public IMapper _mapper;
        public ReportsController(ReportingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [AllowAnonymous]
        // GET: api/Reports
        [HttpGet]
        public async Task<List<Report>> GetReports()
        {
            return await _context.Reports.ToListAsync();
        }

        // PUT: api/PutReport
        [HttpPut]
        public async Task<ActionResult> PutReport(UpdateReport report)
        {
            Report Reports = _mapper.Map<Report>(report);
            var results = new List<ValidationResult>();
            var context = new System.ComponentModel.DataAnnotations.ValidationContext(report);
            if (!Validator.TryValidateObject(report, context, results, true))
            {
                foreach (var error in results)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            _context.Entry(Reports).State = EntityState.Modified;

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

        // PUT: api/PutParentId
        [HttpPut("{id}")]
        public async Task<ActionResult> PutParentId(int id, [FromBody] int? parentId)
        {
            var report = await _context.Reports.FindAsync(id);
            report.ParentId = parentId;
            _context.Reports.Update(report);

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

            try
            {
                _context.Reports.Remove(report);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.InnerException.Message);
            }
        }
    }
}
