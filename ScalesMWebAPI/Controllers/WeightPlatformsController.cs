using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScalesMWebAPI.Dtos;
using ScalesMWebAPI.Services.Handlers;
using Swashbuckle.AspNetCore.Annotations;

namespace ScalesMWebAPI.Models
{
    [SwaggerTag("Назначение платформы от определенного контроллера, определенной точке взвешивания")]
    [Route("api/[controller]")]
    [ApiController]
    public class WeightPlatformsController : ControllerBase
    {
        private readonly KRRPAMONSCALESContext _context;
        private IMapper _mapper;
        public WeightPlatformsController(KRRPAMONSCALESContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/WeightPlatforms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeightPlatform>>> GetWeightPlatforms()
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated )
            {
                return await _context.WeightPlatforms.ToListAsync();
            }
            else
            {
                return Unauthorized();
            }
        }

        // GET: api/WeightPlatforms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WeightPlatform>> GetWeightPlatform(int id)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated )
            {
                var weightPlatform = await _context.WeightPlatforms.FindAsync(id);

            if (weightPlatform == null)
            {
                return NotFound();
            }

            return weightPlatform;
            }
            else
            {
                return Unauthorized();
            }
        }

        // PUT: api/WeightPlatforms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWeightPlatform(int id, UpdateWeightPlatformDto weightPlatform)
        {
            
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated )
            {
                WeightPlatform dbData = _mapper.Map<WeightPlatform>(weightPlatform);
                if (id != dbData.Id)
            {
                return BadRequest();
            }
                _context.Entry(dbData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeightPlatformExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
            }
            else
            {
                return Unauthorized();
            }
        }

        // POST: api/WeightPlatforms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostWeightPlatform(AddWeightPlatformDto weightPlatform)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated )
            {
                var select_ = _context.WeightPlatforms.Where(w => w.ScaleNumberPlatform == weightPlatform.ScaleNumberPlatform && w.WeightPlcPlatform == weightPlatform.WeightPlcPlatform
                                && w.WeightPlcId == weightPlatform.WeightPlcId
                                && w.WeightPointId == weightPlatform.WeightPointId).Count();
                if (select_ > 0)
                {
                    return BadRequest("Запрещено создавать дубликаты");
                }
                else 
                {
                    WeightPlatform dbData = _mapper.Map<WeightPlatform>(weightPlatform);
                    _context.Entry(dbData).State = EntityState.Added;
                    _context.WeightPlatforms.Add(dbData);
                    await _context.SaveChangesAsync();

                    return Ok();// CreatedAtAction("GetWeightPlatform", new { id = dbData.Id }, dbData);
                }
            }
            else
            {
                return Unauthorized();
            }
        }

        // DELETE: api/WeightPlatforms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeightPlatform(int id)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated )//&& Security.IsInGroup(base.User, "EUROPE\\KRR-LG-PA-LabelPrn_Marker"))
            {
                var weightPlatform = await _context.WeightPlatforms.FindAsync(id);
                if (weightPlatform == null)
                {
                    return NotFound();
                }

                _context.WeightPlatforms.Remove(weightPlatform);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            else
            {
                return Unauthorized();
            }
        }

        private bool WeightPlatformExists(int id)
        {
            return _context.WeightPlatforms.Any(e => e.Id == id);
        }
    }
}
