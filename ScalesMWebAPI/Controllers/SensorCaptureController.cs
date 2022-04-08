using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScalesMWebAPI.Dtos;
using ScalesMWebAPI.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace ScalesMWebAPI.Controllers
{
    [SwaggerTag("Таблица значений сенсоров весового оборудования")]
    [EnableCors("AllowSpecificOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class SensorCaptureController : ControllerBase
    {
        private readonly KRRPAMONSCALESContext _context;
        private readonly IMapper _mapper;
        public SensorCaptureController(KRRPAMONSCALESContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/SensorCaptureByDateAndIdScales
        [HttpGet("{id_wp},{start_date},{end_date}")]
        public async Task<ActionResult<IEnumerable<SensorCapture>>> GetSensorCaptureByDateAndIdScales(int id_wp, long start_date, long end_date)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var async_select = _context.SensorCaptures
                    .Where(x =>
                        x.WeightPointId == id_wp &&
                        (x.Dt >= DateTimeOffset.FromUnixTimeSeconds(start_date).DateTime.ToLocalTime() &&
                         x.Dt <= DateTimeOffset.FromUnixTimeSeconds(end_date).DateTime.ToLocalTime())
                    ).ToListAsync();
                return await async_select;
                //await _context.KepMonitoringWeightArchives.Select(x => x.Dt >= bg_date && x.Dt <= end_date).ToListAsync();
                //_context.KepMonitoringWeightArchives.ToListAsync();
            }
            else
            {
                return Unauthorized();
            }
        }

        //// GET: api/SensorCapture/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<KepMonitoringWeightArchive>> GetSensorCapture(long id)
        //{
        //    if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
        //    {
        //        var kepMonitoringWeightArchive = await _context.KepMonitoringWeightArchives.FindAsync(id);

        //    if (kepMonitoringWeightArchive == null)
        //    {
        //        return NotFound();
        //    }

        //    return kepMonitoringWeightArchive;
        //    }
        //    else
        //    {
        //        return Unauthorized();
        //    }
        //}

        // GET: api/KepMonitoringWeightArchives/5
        [HttpGet("last/{id_wp}")]
        public async Task<ActionResult<GetSensorValueDto>> GetLastSensorDto(int id_wp)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {

                return await Task<List<GetSensorValueDto>>.Run(() =>
                {
                    
                    GetSensorValueDto res = new GetSensorValueDto();

                    var select_WeightPoints = (from item in _context.WeightPoints
                                               where item.Id == id_wp
                                               select item);

                    int id_WP = select_WeightPoints.Select(x => x.Id).FirstOrDefault();
                    List<PlatformSensorValueDto> Platforms = new List<PlatformSensorValueDto>();
                    res.Platforms = new List<PlatformSensorValueDto>();
                    if (id_WP > 0)
                    {
                        res.Weight_PointId = id_wp;
                        
                        PlatformSensorValueDto platform = new PlatformSensorValueDto();
                        var select_WeightPlatforms = (from item in _context.WeightPlatforms
                                                      where item.WeightPointId == id_WP
                                                      select item);
                        List<WeightPlatform> weightPlatforms = select_WeightPlatforms.ToList();
                        foreach (var plat in weightPlatforms)
                        {
                            var select_max_date = (from item in _context.SensorCaptures
                                                   where item.WeightPointId == id_wp && item.WeightPlcid == plat.Id
                                                   select item).OrderByDescending(x => x.Dt);
                            DateTime max_date = select_max_date.Select(x => x.Dt).FirstOrDefault();
                            if (max_date != DateTime.MinValue)
                            {
                                SensorCapture select_row = select_max_date.Where(x => x.Dt == max_date).FirstOrDefault();
                                if (select_row.Stabilization == null || select_row.Stabilization == "") { select_row.Stabilization = "False"; }
                                platform = _mapper.Map<PlatformSensorValueDto>(select_row);
                                platform.Weight_PLCId = plat.WeightPlcId;
                                res.Platforms.Add(platform);
                            }
                        }
                    }
                    return res;
                });
            }
            else
            {
                return Unauthorized();
            }
        }

        //// PUT: api/KepMonitoringWeightArchives/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutKepMonitoringWeightArchive(long id, KepMonitoringWeightArchive kepMonitoringWeightArchive)
        //{
        //    if (id != kepMonitoringWeightArchive.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(kepMonitoringWeightArchive).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!KepMonitoringWeightArchiveExists(id))
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

        // POST: api/KepMonitoringWeightArchives
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost()]
        public async Task<ActionResult<AddSensorValueDto>> PostSensorCapture(AddSensorValueDto sensorValue)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var select_WeightPoints = (from item in _context.WeightPoints
                                    where item.FkExternalSystem == sensorValue.UWSScalesId
                                    select item);
                int id_WP = 0;
                try
                {
                    id_WP = await select_WeightPoints.Select(x => x.Id).FirstOrDefaultAsync();
                }
                catch (Exception)
                {
                    return BadRequest("Not found UWSScalesID - "+ sensorValue.UWSScalesId);
                }
                if (id_WP > 0)
                {
                    var select_WeightPlatforms = (from item in _context.WeightPlatforms
                                                  where item.WeightPointId == id_WP && item.ScaleNumberPlatform == sensorValue.PlatformN
                                                  select item);
                    int plcId = 0;
                    plcId = (int)await select_WeightPlatforms.Select(x => x.Id).FirstOrDefaultAsync();
                    if (plcId > 0)
                    {
                        SensorCapture dbData = _mapper.Map<SensorCapture>(sensorValue);
                        dbData.Stabilization = sensorValue.Stabilization.ToString();
                        dbData.WeightPlcid = plcId;
                        dbData.WeightPointId = id_WP;
                        if (dbData.Stabilization == null || dbData.Stabilization == "") { dbData.Stabilization = "False"; }
                        dbData.Dt = DateTime.Now;
                        dbData.DtUtc = DateTime.Now.ToUniversalTime();
                        _context.SensorCaptures.Add(dbData);
                        if (ModelState.IsValid)
                        {
                            try
                            {
                                await _context.SaveChangesAsync();
                            }
                            catch (Exception)
                            {
                                return BadRequest(ModelState);
                            }

                        }
                        else
                        {
                            return BadRequest(ModelState);
                        }
                    }
                    else
                    {
                        return BadRequest("Not found PLC - " + sensorValue.UWSScalesId);
                    }
                }
                else 
                {
                    return BadRequest("Not found WeightPointId - " + sensorValue.UWSScalesId);
                }
                return Ok(200);
                //return CreatedAtAction("KepMonitoringWeightArchives/monitor/", new { id = dbData.Id });
            }
            else
            {
                return Unauthorized();
            }
        }

        //// DELETE: api/KepMonitoringWeightArchives/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteKepMonitoringWeightArchive(long id)
        //{
        //    var kepMonitoringWeightArchive = await _context.KepMonitoringWeightArchives.FindAsync(id);
        //    if (kepMonitoringWeightArchive == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.KepMonitoringWeightArchives.Remove(kepMonitoringWeightArchive);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool SensorCaptureExists(long id)
        {
            return _context.SensorCaptures.Any(e => e.Id == id);
        }
    }
}
