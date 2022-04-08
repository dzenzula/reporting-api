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
    [Route("api/[controller]")]
    [ApiController]
    public class WeightSensorsController : ControllerBase
    {
        private readonly KRRPAMONSCALESContext _context;
        private readonly IMapper _mapper;

        public WeightSensorsController(KRRPAMONSCALESContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //// GET: api/WeightSensors
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<WeightSensor>>> GetWeightSensors()
        //{
        //    if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
        //    {
        //        return await _context.WeightSensors.ToListAsync();
        //    }
        //    else 
        //    {
        //        return Unauthorized();
        //    }
        //}

        // GET: api/WeightSensors/5
        [HttpGet("{id_wplc}")]
        public async Task<ActionResult<GetWeightSensorDto>> GetWeightSensor(int id_wplc)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                return await Task<List<GetWeightSensorDto>>.Run(() =>
                {
                    GetWeightSensorDto res = new GetWeightSensorDto();

                    var select_WeightSensors = (from item in _context.WeightSensors
                                               where item.WeightPlcid == id_wplc
                                               select item);
                    int id_WP = select_WeightSensors.Select(x => x.WeightPlcid).FirstOrDefault();
                    if (id_WP > 0)
                    {
                        res.WeightPlcid = id_WP;
                        WeightSensorDto sensors = new WeightSensorDto();
                        List<WeightSensor> weightSensors = select_WeightSensors.ToList();
                        res.Sensors = new List<WeightSensorDto>();
                        foreach (var item in weightSensors)
                        {
                            WeightSensorDto sensor = new WeightSensorDto();
                            sensor.id = item.Id;
                            sensor.ServiceTag = item.ServiceTag;
                            sensor.DtInstall = item.DtInstall;
                            res.Sensors.Add(sensor);
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

        // PUT: api/WeightSensors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{old_ServiceTag}")]
        public async Task<IActionResult> PutWeightSensor(string old_ServiceTag, UpdateWeightSensorDataDto updateSensor)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                if (old_ServiceTag != updateSensor.OldServiceTag)
                {
                    return BadRequest();
                }
                WeightSensor dbData = _mapper.Map<WeightSensor>(updateSensor);
                
                var select_WeightSensors = (from item in _context.WeightSensors
                                            where item.ServiceTag == updateSensor.OldServiceTag && item.WeightPlcid == updateSensor.WeightPlcid
                                            select item);
                dbData.Id = select_WeightSensors.Select(x => x.Id).FirstOrDefault();
                if (!WeightSensorExists(dbData.Id))
                {
                    return NotFound();
                }
                else
                {
                    try
                    {
                        dbData.ServiceTag = updateSensor.NewServiceTag;
                        dbData.DtInstall = updateSensor.DtWork;
                        _context.Entry(dbData).State = EntityState.Modified;
                        try
                        {
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateException e)
                        {

                            return BadRequest(e.InnerException.Message);
                        }
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return BadRequest();
                    }
                }
            return NoContent();
            }
            else
            {
                return Unauthorized();
            }
        }

        // POST: api/WeightSensors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AddWeightSensorDto>> PostWeightSensor(AddWeightSensorDto addSensor)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                WeightSensor dbData = _mapper.Map<WeightSensor>(addSensor);
                _context.WeightSensors.Add(dbData);
                _context.Entry(dbData).State = EntityState.Added;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {

                    return BadRequest(e.InnerException.Message);
                }
            

            return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        // DELETE: api/WeightSensors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeightSensor(int id)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var weightSensor = await _context.WeightSensors.FindAsync(id);
            if (weightSensor == null)
            {
                return NotFound();
            }

            _context.WeightSensors.Remove(weightSensor);
            await _context.SaveChangesAsync();

            return NoContent();
            }
            else
            {
                return Unauthorized();
            }
        }

        private bool WeightSensorExists(int id)
        {
            return _context.WeightSensors.Any(e => e.Id == id);
        }
    }
}
