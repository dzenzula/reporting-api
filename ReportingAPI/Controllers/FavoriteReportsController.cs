using AuthorizationApiHandler.PolicysAuthorize;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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
    [SwaggerTag("Получить избранные отчеты")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [MultiplePolicysAuthorize("work_with_favorite_reports")]
    public class FavoriteReportsController : ControllerBase
    {
        IHttpContextAccessor _httpContextAccessor = null;
        private readonly ReportingContext _context;
        private IMapper _mapper;

        public FavoriteReportsController(IHttpContextAccessor httpContextAccessor, ReportingContext context, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
        }

        // GET: api/FavoriteReports/GetReports
        [HttpGet]
        public async Task<List<Report>> GetReports()
        {
            string Login = GetLogin();
            return await _context.FavoriteReports.Where(y => y.Login == Login).Select(y => y.Report).ToListAsync();
        }
        // POST: api/FavoriteReports/AddReport
        [HttpPost("{ReportId}")]
        public async Task<ActionResult> AddReport(int ReportId)
        {
            var t = ReportId;
            Report report = _context.Reports.FirstOrDefault(x => x.Id == ReportId);
            if (report is null)
                return BadRequest("Отчет с указанным Id не найден!");
            FavoriteReport favoriteReport = await _context.FavoriteReports.FindAsync(ReportId);
            if (favoriteReport is not null)
                return BadRequest("Данный отчет уже добавлен в избранные!");
            FavoriteReport newFavoriteReport = new FavoriteReport
            {
                ReportId = ReportId,
                Login = GetLogin()
            };

            try
            {
                _context.FavoriteReports.Add(newFavoriteReport);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.InnerException.Message);
            }
        }
        //DELETE: api/FavoriteReports/DeleteReport
        [HttpDelete("{ReportId}")]
        public async Task<ActionResult> DeleteReport(int ReportId)
        {
            Report report = _context.Reports.FirstOrDefault(x => x.Id == ReportId);
            if (report is null)
                return BadRequest("Отчет с указанным Id не найден!");
            string Login = GetLogin();
            FavoriteReport favoriteReport = await _context.FavoriteReports
                .Where(x => x.Login == Login && x.ReportId == ReportId)
                .FirstOrDefaultAsync();
            if (favoriteReport is null)
                return BadRequest("Данный отчет не добавлен в избранные!");
            try
            {
                _context.FavoriteReports.Remove(favoriteReport);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
        }

        private string GetLogin()
        {
            string Name = _httpContextAccessor.HttpContext.Session.GetString("USER_DOMAIN_NAME");
            string Domain = _httpContextAccessor.HttpContext.Session.GetString("USER_DOMAIN");

            return Domain + @"\" + Name;
        }
    }
}
