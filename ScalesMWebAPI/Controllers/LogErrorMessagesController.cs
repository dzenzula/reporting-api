using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScalesMWebAPI.Dtos;
using ScalesMWebAPI.Models;

namespace ScalesMWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LogErrorMessagesController : ControllerBase
    {
        private readonly KRRPAMONSCALESContext _context;
        private readonly IMapper _mapper;

        public LogErrorMessagesController(KRRPAMONSCALESContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/LogErrorMessages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LogErrorMessage>>> GetLogErrorMessages()
        {
            return await _context.LogErrorMessages.ToListAsync();
        }

        // GET: api/LogErrorMessages/5
        [HttpGet("{id_message}")]
        public async Task<ActionResult<LogErrorMessage>> GetLogErrorMessage(long id_message)
        {
            var logErrorMessage = await _context.LogErrorMessages.FindAsync(id_message);

            if (logErrorMessage == null)
            {
                return NotFound();
            }

            return logErrorMessage;
        }
        // GET: api/LogErrorMessages/5
        [HttpGet("{id_wp}")]
        public async Task<ActionResult<List<LogErrorMessage>>> GetLogErrorMessagePlc(long id_wp)
        {
            var logErrorMessage = await _context.LogErrorMessages.Where(x => x.WeightPointId == id_wp).ToListAsync();

            if (logErrorMessage == null)
            {
                return NotFound();
            }

            return logErrorMessage;
        }
        //// PUT: api/LogErrorMessages/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutLogErrorMessage(long id, LogErrorMessage logErrorMessage)
        //{
        //    if (id != logErrorMessage.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(logErrorMessage).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!LogErrorMessageExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/LogErrorMessages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LogErrorMessage>> PostLogErrorMessage(AddLogErrorMessageDto logErrorMessage)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var select_WeightPoints = (from item in _context.WeightPoints
                                           where item.FkExternalSystem == logErrorMessage.UWSScalesId
                                           select item);
                int id_WP = 0;
                try
                {
                    id_WP = await select_WeightPoints.Select(x => x.Id).FirstOrDefaultAsync();
                }
                catch (Exception)
                {
                    return BadRequest("Not found UWSScalesID - " + logErrorMessage.UWSScalesId);
                }
                if (id_WP > 0)
                {
                    LogErrorMessage mes = _mapper.Map<LogErrorMessage>(logErrorMessage);
                    mes.WeightPointId = id_WP;
                    mes.DtError = DateTime.Now;
                    mes.DtInsert = DateTime.Now;
                    mes.DtUpdate = DateTime.Now;
                    _context.LogErrorMessages.Add(mes);
                    await _context.SaveChangesAsync();
                    return Ok(200);
                }
                else 
                {
                    return BadRequest("Not found UWSScalesID - " + logErrorMessage.UWSScalesId);
                }
            }
            else
            {
                return Unauthorized();
            }

        }

        //// DELETE: api/LogErrorMessages/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteLogErrorMessage(long id)
        //{
        //    var logErrorMessage = await _context.LogErrorMessages.FindAsync(id);
        //    if (logErrorMessage == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.LogErrorMessages.Remove(logErrorMessage);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool LogErrorMessageExists(long id)
        {
            return _context.LogErrorMessages.Any(e => e.Id == id);
        }
    }
}
