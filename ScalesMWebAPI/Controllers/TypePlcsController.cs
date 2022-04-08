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
    [SwaggerTag("Тип контроллера (АЦП/ Контроллер и АЦП)")]
    [Route("api/[controller]")]
    [ApiController]
    public class TypePlcsController : ControllerBase
    {
        private readonly KRRPAMONSCALESContext _context;
        private readonly IMapper _mapper;

        public TypePlcsController(KRRPAMONSCALESContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        /// <summary>
        /// Retrieves all name Type PLC 
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Product created</response>
        /// <response code="400">Product has missing/invalid values</response>
        /// <response code="500">Oops! Can't create your product right now</response>
        // GET: api/TypePlcs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypePlc>>> GetTypePlcs()
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {

                return await _context.TypePlcs.ToListAsync();
            }
            else
            {
                return Unauthorized();
            }
        }

        // GET: api/TypePlcs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TypePlc>> GetTypePlc(int id)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var typePlc = await _context.TypePlcs.FindAsync(id);

            if (typePlc == null)
            {
                return NotFound();
            }

            return typePlc;
            }
            else
            {
                return Unauthorized();
            }
        }

        // PUT: api/TypePlcs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypePlc(int id, TypePlc typePlc)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                if (id != typePlc.Id)
            {
                return BadRequest();
            }
                _context.Entry(typePlc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypePlcExists(id))
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

        // POST: api/TypePlcs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //[Authorized(Role = "Write_Data")]
        public async Task<ActionResult> PostTypePlc(AddTypePlcDto typePlc)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var select_ = _context.TypePlcs.Where(w => w.NameType.ToLower().Trim() == typePlc.NameType.ToLower().Trim()).Count();
                if (select_ > 0) 
                {
                    return BadRequest("Запрещено создавать дубликаты"); 
                }
                else
                {
                    try
                    {
                        TypePlc tp = _mapper.Map<TypePlc>(typePlc);
                        _context.TypePlcs.Add(tp);
                        await _context.SaveChangesAsync();

                        return Ok();//CreatedAtAction("GetTypePlc", new { id = typePlc.Id }, typePlc);
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

        //// DELETE: api/TypePlcs/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTypePlc(int id)
        //{
        //    var typePlc = await _context.TypePlcs.FindAsync(id);
        //    if (typePlc == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.TypePlcs.Remove(typePlc);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool TypePlcExists(int id)
        {
            return _context.TypePlcs.Any(e => e.Id == id);
        }
    }
}
