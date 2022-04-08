using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScalesMWebAPI.Dtos;
using ScalesMWebAPI.Models;
using ScalesMWebAPI.Services.Handlers;
using Swashbuckle.AspNetCore.Annotations;

namespace ScalesMWebAPI.Controllers
{
    /// <summary>Service for providing operations for <cref>Assigment definition</cref></summary>
    [SwaggerTag("Назначение точки взвешивания (авто/жд. и т.д.)")]
    [Route("api/[controller]")]
    [ApiController]
    public class AssigmentPointsController : ControllerBase
    {
        private readonly KRRPAMONSCALESContext _context;
        private readonly IMapper _mapper;

        public AssigmentPointsController(KRRPAMONSCALESContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        /// <summary>
        /// Retrieves all name Assigment definition 
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Product list</response>
        /// <response code="400">Assigment definition has missing values</response>
        /// <response code="401">User has no unauthorized</response>
        /// <response code="500">Oops! Can't create your product right now</response>
        // GET: api/AssigmentPoints
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAssigmentPointDto>>> GetAssigmentPoints()
        {
            //if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated) //&& Security.IsInGroup(base.User, "EUROPE\\KRR-LG-PA-LabelPrn_Marker")
            //{
            return await Task<ActionResult<List<GetAssigmentPointDto>>>.Run(() =>
            {
                List<GetAssigmentPointDto> l_ap = new List<GetAssigmentPointDto>();
                var sel_ap = _context.AssigmentPoints.ToList();
                foreach (var item in sel_ap)
                {
                    l_ap.Add(_mapper.Map<GetAssigmentPointDto>(item));
                }
                return l_ap;
            });
            //}
            //else
            //{
            //    return Unauthorized();
            //}
        }
        /// <summary>
        /// Retrieves a specific name Assigment definition by unique id
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Product list </response>
        /// <response code="400">Product list has missing</response>
        /// <response code="500">Oops! Can't create your product right now</response>
        // GET: api/AssigmentPoints/5
        [HttpGet("{id}")]

        public async Task<ActionResult<AssigmentPoint>> GetAssigmentPoint(int id)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var assigmentPoint = await _context.AssigmentPoints.FindAsync(id);

                if (assigmentPoint == null)
                {
                    return NotFound();
                }

                return assigmentPoint;
            }
            else
            {
                return Unauthorized();
            }
        }
        /// <summary>
        /// Update a specific name Assigment definition by unique id
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Product created</response>
        /// <response code="400">Product has missing/invalid values</response>
        /// <response code="500">Oops! Can't create your product right now</response>
        // PUT: api/AssigmentPoints/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id},{NameAssigment}")]
        public async Task<IActionResult> PutAssigmentPoints(int id, string NameAssigment)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var assigmentPoint = await _context.AssigmentPoints.FindAsync(id);
                if (id != assigmentPoint.Id)
                {
                    return BadRequest();
                }
                var count_asigment = _context.WeightPoints.Where(s => s.AssigmentPointId == assigmentPoint.Id).Count();
                if (count_asigment > 0)
                {
                    return BadRequest("Есть весовые точки этого типа назначения.");
                }

                _context.Entry(assigmentPoint).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssigmentPointExists(id))
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
        /// <summary>
        /// Create a new Assigment definition 
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Product created</response>
        /// <response code="400">Product has missing/invalid values</response>
        /// <response code="500">Oops! Can't create your product right now</response>
        // POST: api/AssigmentPoints
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostAssigmentPoint(AddAssigmentPointDto assigmentPoint)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                try
                {
                    var count_asigment = _context.AssigmentPoints.Where(s => s.NameAssigment == assigmentPoint.NameAssigment.Trim()).Count();
                    if (count_asigment > 0)
                    {
                        return BadRequest("Запрещено создавать дубликаты типов назначения.");
                    }

                    AssigmentPoint ap = _mapper.Map<AssigmentPoint>(assigmentPoint);
                    _context.AssigmentPoints.Add(ap);
                    await _context.SaveChangesAsync();

                    return Ok(200); //CreatedAtAction("GetAssigmentPoint", new { id = assigmentPoint.Id }, assigmentPoint);

                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            else
            {
                return Unauthorized();
            }
        }
        /// <summary>
        /// DELETE a specific Assigment definition by unique id
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Product created</response>
        /// <response code="400">Product has missing/invalid values</response>
        /// <response code="500">Oops! Can't update your product right now</response>
        // DELETE: api/AssigmentPoints/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssigmentPoint(int id)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {

                var assigmentPoint = await _context.AssigmentPoints.FindAsync(id);
                if (assigmentPoint == null)
                {
                    return NotFound();
                }
                var count_asigment = _context.WeightPoints.Where(s => s.AssigmentPointId == assigmentPoint.Id).Count();
                if (count_asigment > 0) 
                {
                    return BadRequest("Есть весовые точки этого типа назначения.");
                }
                _context.AssigmentPoints.Remove(assigmentPoint);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            else
            {
                return Unauthorized();
            }
        }

        private bool AssigmentPointExists(int id)
        {
            return _context.AssigmentPoints.Any(e => e.Id == id);
        }
    }
}
