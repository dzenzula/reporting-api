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
using Swashbuckle.AspNetCore.Annotations;

namespace ScalesMWebAPI.Controllers
{
    [SwaggerTag("Основная сущность - точка взвешивания, определеяет - где размещенно, что именно размещенно, какое назначение точки взвешивания")]
    [Route("api/[controller]")]
    [ApiController]
    public class WeightPointsController : ControllerBase
    {
        private readonly KRRPAMONSCALESContext _context;
        private readonly IMapper _mapper;

        public WeightPointsController(KRRPAMONSCALESContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/WeightPoints
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeightPointExtended>>> GetWeightPoints()
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                return await Task<List<WeightPointExtended>>.Factory.StartNew(() => {
                    List<WeightPointExtended> wpe = new List<WeightPointExtended>();
                    var WPoints = _context.WeightPoints.ToList();
                    foreach (var item in WPoints)
                    {
                        WeightPointExtended wpeo = new WeightPointExtended(item);
                        wpeo.Status = WorkStatus.Info;
                        wpe.Add(wpeo);
                    }
                    return wpe;
                });
                //return await _context.WeightPoints.ToListAsync();
        }
            else
            {
                return Unauthorized();
    }
}

        // GET: api/WeightPoints/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WeightPoint>> GetWeightPoint(int id)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var weightPoint = await _context.WeightPoints.FindAsync(id);

            if (weightPoint == null)
            {
                return NotFound();
            }

            return weightPoint;
            }
            else
            {
                return Unauthorized();
            }
        }

        // PUT: api/WeightPoints/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWeightPoint(int id, UpdateWeightPointDto weightPoint)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                if (id != weightPoint.Id)
            {
                return BadRequest();
            }
                WeightPoint wp_origin = _context.WeightPoints.Find(weightPoint.Id);
                wp_origin.AssigmentPointId = weightPoint.AssigmentPointId;
                wp_origin.LocationPointId = weightPoint.LocationPointId;
                wp_origin.NumberScale = weightPoint.NumberScale;
                wp_origin.NamePoint = weightPoint.NamePoint;
                wp_origin.DtUpdate = DateTime.Now;
                _context.Entry(wp_origin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeightPointExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
                return Ok(200);
            }
            else
            {
                return Unauthorized();
    }
}

        // POST: api/WeightPoints
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostWeightPoint(AddWeightPointDto weightPoint)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var select_ = _context.WeightPoints.Where(w => w.NumberScale.ToLower().Trim() == weightPoint.NumberScale.ToLower().Trim() || w.NamePoint.ToLower().Trim() == weightPoint.NamePoint.ToLower().Trim()).Count();
                if (select_ > 0)
                {
                    return BadRequest("Запрещено создавать дубликаты");
                }
                else 
                {
                    try
                    {
                        WeightPoint wp = _mapper.Map<WeightPoint>(weightPoint);
                        _context.WeightPoints.Add(wp);
                        await _context.SaveChangesAsync();

                        return Ok();//CreatedAtAction("GetWeightPoint", new { id = weightPoint.Id }, weightPoint);
                    }
                    catch (Exception ex) 
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
            else
            {
                return Unauthorized();
            }
        }

        // DELETE: api/WeightPoints/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeightPoint(int id)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var weightPoint = await _context.WeightPoints.FindAsync(id);
                if (weightPoint == null)
                {
                    return NotFound();
                }
                var count_asigment = _context.WeightPlcs.Where(s => s.ScalesNumberId == id).Count();
                var tag_plc = _context.WeightPlcs.Where(s => s.ScalesNumberId == id);
                    var tag = _context.WeightPlcs.Where(s => s.ScalesNumberId == id).Select(s => s.ServiceTag).FirstOrDefault();
                if (count_asigment > 0)
                {
                    return BadRequest($"К данной весовой назначен весовой контроллер {tag}.");
                }
                else
                {
                    _context.WeightPoints.Remove(weightPoint);
                    await _context.SaveChangesAsync();

                    return NoContent();
                }
            } 
            else
            {
                return Unauthorized();
            }
        }

        private bool WeightPointExists(int id)
        {
            return _context.WeightPoints.Any(e => e.Id == id);
        }
    }
}
