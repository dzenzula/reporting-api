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
    [SwaggerTag("Описание весового котнроллера - назначается к точке взвешивания, название контроллера, тип контроллера, уникальный номер")]
    [Route("api/[controller]")]
    [ApiController]
    public class WeightPlcsController : ControllerBase
    {
        private readonly KRRPAMONSCALESContext _context;
        private readonly IMapper _mapper;

        public WeightPlcsController(KRRPAMONSCALESContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/WeightPlcs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeightPlc>>> GetWeightPlcs()
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                return await _context.WeightPlcs.ToListAsync();
            }
            else
            {
                return Unauthorized();
            }
}

        // GET: api/WeightPlcs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WeightPlc>> GetWeightPlc(int id)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var weightPlc = await _context.WeightPlcs.FindAsync(id);

            if (weightPlc == null)
            {
                return NotFound();
            }

            return weightPlc;
            }
            else
            {
                return Unauthorized();
            }
        }

        // PUT: api/WeightPlcs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult> PutWeightPlc(int id, UpdateWeightPlcDto weightPlc)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                if (WeightPlcExists(weightPlc.Id))
                {
                    try
                    {
                        WeightPlc wplc = _mapper.Map<WeightPlc>(weightPlc);
                        _context.WeightPlcs.Update(wplc);
                        await _context.SaveChangesAsync();
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
                else 
                {
                    return NotFound();
                }
            }
            else
            {
                return Unauthorized();
            }
        }

        // POST: api/WeightPlcs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostWeightPlc(AddWeightPlcDto weightPlc)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var select_ = _context.WeightPlcs.Where(w => w.NamePlc.ToLower().Trim() == weightPlc.NamePlc.ToLower().Trim()).Count();
                if (select_ > 0)
                {
                    return BadRequest("Запрещено создавать дубликаты");
                }
                else
                {
                    try
                    {
                        WeightPlc wplc = _mapper.Map<WeightPlc>(weightPlc);
                        _context.WeightPlcs.Add(wplc);
                        await _context.SaveChangesAsync();

                        return Ok();//CreatedAtAction("GetWeightPlc", new { id = weightPlc.Id }, weightPlc);
                    }
                    catch (Exception)
                    {
                        return BadRequest();
                    }
                }
            }
            else
            {
                return Unauthorized();
            }
        }

        // DELETE: api/WeightPlcs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeightPlc(int id)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var weightPlc = await _context.WeightPlcs.FindAsync(id);
                if (weightPlc == null)
                {
                    return NotFound();
                }
                var count_asigment = _context.WeightPlatforms.Where(s => s.WeightPlcId == id).Count();
                if (count_asigment > 0)
                {
                    return BadRequest($"К данному контроллеру назначены весовые платформы.");
                }
                else
                {
                    _context.WeightPlcs.Remove(weightPlc);
                    await _context.SaveChangesAsync();

                    return NoContent();
                }
            }
            else
            {
                return Unauthorized();
            }
        }

        private bool WeightPlcExists(int id)
        {
            return _context.WeightPlcs.Any(e => e.Id == id);
        }
    }
}
