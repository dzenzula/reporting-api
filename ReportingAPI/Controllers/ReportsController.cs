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
            List<Report> reports = await _context.Reports.Include(x => x.Categories).AsNoTracking().ToListAsync();
            List<Report> outRep = new();

            foreach (var report in reports)
                foreach (var category in report.Categories)
                {
                    Report temp = report.Clone();
                    temp.ParentId = category.Id;
                    outRep.Add(temp);
                }

            return outRep;
            //     ToListAsync();
        }

        // PUT: api/PutReport
        [HttpPut]
        public async Task<ActionResult> PutReport(UpdateReport report)
        {
            Report Reports = await _context.Reports.FirstOrDefaultAsync(x => x.Id == report.Id);
            var results = new List<ValidationResult>();
            var context = new System.ComponentModel.DataAnnotations.ValidationContext(report);
            if (!Validator.TryValidateObject(report, context, results, true))
            {
                foreach (var error in results)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            var DuplicateByText = _context.Reports.Where(x => x.Text == report.Text && x.Id != report.Id);
            var hasDuplicateByText = DuplicateByText.Count(x => x.Text == report.Text && x.Id != report.Id) > 0;
            var DuplicateByURL = _context.Reports.Where(x => x.URL == report.URL && x.Id != report.Id);
            var hasDuplicateByURL = DuplicateByURL.Count(x => x.URL == report.URL && x.Id != report.Id) > 0;
            string Alias = "";

            if (hasDuplicateByText)
            {
                Alias = DuplicateByText.First().Alias;
                return BadRequest($"Отчет с таким именем уже существует.\nПсевдоним отчета: {Alias}");
            }
            else if (hasDuplicateByURL)
            {
                Alias = DuplicateByURL.First().Alias;
                return BadRequest($"Отчет с такой ссылкой уже существует.\nПсевдоним отчета: {Alias}");
            }

            Reports.Text = report.Text;
            Reports.URL = report.URL;
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
        //[AllowAnonymous]
        [HttpPut("UpdateCategoryReports")]
        ///api/Reports/UpdateCategoryReports
        public async Task<ActionResult> PutParentId(UpdateCategoryReports CategoryReports = null)
        {
            Report report = _context.Reports.Include(x => x.Categories).FirstOrDefault(x => x.Id == CategoryReports.id && x.Categories.Any(y => y.Id == CategoryReports.fromCat));
            Category fromCategory = report.Categories.FirstOrDefault(y => y.Id == CategoryReports.fromCat);
            if (report == null || fromCategory == null)
            {
                return BadRequest("Отчета с данным Id в указанной категории не существует");
            }
            Console.WriteLine(CategoryReports.toCat);
            var HasDublicateInCat = report.Categories.Any(x => x.Id == CategoryReports.toCat);

            if (HasDublicateInCat)
            {
                return BadRequest("В данной категории уже существует такой отчет. Размещение одинаковых отчетов в одной категории запрещено.");
            }

            //var t = _context.Reports.Include(x => x.Categories).AsNoTracking().FirstOrDefault(x => CategoryReports.id == x.Id && x.Categories.Any(y => y.Id == CategoryReports.toCat));
            //Console.WriteLine();
            report.Categories.Remove(fromCategory);

            Category toCategory = _context.Categories.Include(x => x.Reports).FirstOrDefault(y => y.Id == CategoryReports.toCat);
            report.Categories.Add(toCategory);

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

        //POST: api/Reports/AddReportRelation
        [HttpPost("AddReportRelation/{id}")]
        public async Task<ActionResult> PostReportRelation(int id, [FromBody] int toCat)
        {
            Report report = _context.Reports.Include(c => c.Categories).FirstOrDefault(x => x.Id == id);
            if (report is null)
                return BadRequest("Отчета с таким id не существует.");

            Category category = _context.Categories.FirstOrDefault(x => x.Id == toCat);
            if (category is null)
                return BadRequest("Указанонй категории не существует.");

            Category dublicateReport = report.Categories.FirstOrDefault(cat => cat.Id == toCat);
            //var matchingReports = report.Categories.Any(cat => cat.Id == toCat);
            if (dublicateReport is not null)
                return BadRequest("В указанной категории данный отчет уже существует.");

            report.Categories.Add(category);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }

            return Ok();

            /*var matchingReportsCount = _context.Reports.Include(x => x.Categories).Where(y => y.Text == reportData.Text || y.Alias == reportData.Alias || y.URL == reportData.URL).Count();



            if (matchingReportsCount == 0)
            {
                Report newReport = _mapper.Map<Report>(reportData);
                var category = _context.Categories.FirstOrDefault(x => x.Id == reportData.ParentId);
                if (category is not null)
                    newReport.Categories.Add(category);
                else
                    return BadRequest("Категории не существует");
            }*/

            

        }

        // POST: api/Reports
        [HttpPost]
        public async Task<ActionResult> PostReport(AddReport reportData)
        {
            var query = _context.Reports.Include(x => x.Categories);

            var matchingUrl = query.FirstOrDefault(y => y.URL == reportData.URL);
            if (matchingUrl is not null)
                return BadRequest($"Отчет с такой ссылкой уже существует под псевдонимом: {matchingUrl.Alias}");

            var matchingAlias = query.FirstOrDefault(y => y.Alias == reportData.Alias);
            if (matchingAlias is not null)
                return BadRequest($"Отчет с таким псевдонимом уже существует.");

            var matchingText = query.FirstOrDefault(y => y.Text == reportData.Text);
            if (matchingText is not null)
                return BadRequest($"Отчет с таким названием уже существует под псевдонимом: {matchingText.Alias}");

            // var matchingReportsCount = _context.Reports.Include(x => x.Categories).Where(y => y.Text == reportData.Text || y.Alias == reportData.Alias || y.URL == reportData.URL).Count();

            //TODO: показать псевдонимы отчетов
            //if(matchingReportsCount != 0)
            //    return BadRequest("Отчет с таким названием/псевдонимом или ссылкой уже существует.");

            var category = _context.Categories.FirstOrDefault(x => x.Id == reportData.ParentId);

            if(category is null)
                return BadRequest("Указанной категории не существует.");

            Report newReport = _mapper.Map<Report>(reportData);
            newReport.Categories.Add(category);
            _context.Reports.Add(newReport);


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }

            return Ok(newReport.Id);

           /* Report report = _mapper.Map<Report>(newReport);
            try
            {
                Category category = _context.Categories.FirstOrDefault(x => x.Id == newReport.ParentId);
                if (category == null)
                {
                    return BadRequest("Категория не существует.");
                }

                report.Categories.Add(category);

                _context.Reports.Add(report);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {

                return BadRequest(e.InnerException.Message);
            }

            return Ok(report.Id);*/
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReport(int id, [FromBody] int fromCat)
        {
            var removingReport = _context.Reports.Include(x => x.Categories).FirstOrDefault(p => p.Id == id);
            if (removingReport is null)
                return BadRequest("Отчета с таким Id не существует.");

            var category = removingReport.Categories.FirstOrDefault(x => x.Id == fromCat);
            if (category is null)
                return BadRequest("Отчета в указанной категории не существует.");

            removingReport.Categories.Remove(category);

            var count = removingReport.Categories.Count();

            if (count == 0)
                _context.Remove(removingReport);

            //var reports = _context.Reports.Include(x => x.Categories).Where(y => y.Categories.Where(p => p.Id == fromCat))

            /*Report report = _context.Reports.Include(x => x.Categories).FirstOrDefault(x => x.Id == CategoryReports.id && x.Categories.Any(y => y.Id == CategoryReports.fromCat));
            Console.WriteLine("Test: " + report.Categories.Count());
            Category fromCategory = report.Categories.FirstOrDefault(y => y.Id == CategoryReports.fromCat);
            if (report == null || fromCategory == null)
            {
                throw new Exception("Отчета с данным Id в указанной категории не существует");
            }
            report.Categories.Remove(fromCategory);

            Category toCategory = _context.Categories.Include(x => x.Reports).FirstOrDefault(y => y.Id == CategoryReports.toCat);
            report.Categories.Add(toCategory);*/

            try
            {
              //  _context.Reports.Remove(report);
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
