using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    [SwaggerTag("Размещение точки взвешивания (станция)")]
    [EnableCors("AllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class LocationPointsController : ControllerBase
    {
        private readonly KRRPAMONSCALESContext _context;
        private readonly IMapper _mapper;

        public LocationPointsController(KRRPAMONSCALESContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        /// <summary>
        /// Retrieves all name Location point 
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Product created</response>
        /// <response code="400">Product has missing/invalid values</response>
        /// <response code="500">Oops! Can't create your product right now</response>
        // GET: api/LocationPoints
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocationPoint>>> GetLocationPoints()
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {

                return await _context.LocationPoints.ToListAsync();
            }
            else
            {
                return Unauthorized();
            }
        }
        
        /// <summary>
        /// Retrieves a specific name Location point by unique id
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Product created</response>
        /// <response code="400">Product has missing/invalid values</response>
        /// <response code="500">Oops! Can't create your product right now</response>
        // GET: api/LocationPoints/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LocationPoint>> GetLocationPoint(int id)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var locationPoint = await _context.LocationPoints.FindAsync(id);

            if (locationPoint == null)
            {
                return NotFound();
            }

            return locationPoint;
            }
            else
            {
                return Unauthorized();
            }
        }
        [HttpGet("name")]
        public async Task<ActionResult<LocationPoint>> GetLocationId(string name_)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var name = HttpContext.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Name)?.Value;
            var locationPoint = await _context.LocationPoints.SingleOrDefaultAsync(x => x.NameLocation == name_);

            if (locationPoint == null)
            {
                return NotFound();
            }

            return locationPoint;
            }
            else
            {
                return Unauthorized();
            }
        }
        /// <summary>
        /// Update a specific name Location point by unique id
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Product created</response>
        /// <response code="400">Product has missing/invalid values</response>
        /// <response code="500">Oops! Can't create your product right now</response>
        // PUT: api/LocationPoints/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocationPoint(int id, UpdateLocationPoint locationPoint)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var select_ = _context.LocationPoints.Where(w => w.NameLocation.ToLower().Trim() == locationPoint.NameLocation.ToLower().Trim()).Count();
                if (select_ > 0)
                {
                    return BadRequest("Запрещено создавать дубликаты");
                }
                else
                {
                    if (LocationPointExists(id))
                    {
                        LocationPoint lp = _mapper.Map<LocationPoint>(locationPoint);
                        lp.Id = id;
                        _context.Entry(lp).State = EntityState.Modified;
                    }
                    else
                    {
                        return NotFound();
                    }
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

                
            }
            else
            {
                return Unauthorized();
            }
        }
        /// <summary>
        /// Create a new Location point 
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Product created</response>
        /// <response code="400">Product has missing/invalid values</response>
        /// <response code="500">Oops! Can't update your product right now</response>
        // POST: api/LocationPoints
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostLocationPoint(AddLocationPointDto locationPoint)
        {
            
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var select_ = _context.LocationPoints.Where(w => w.NameLocation.ToLower().Trim() == locationPoint.NameLocation.ToLower().Trim()).Count();
                if (select_ > 0)
                {
                    return BadRequest("Запрещено создавать дубликаты");
                }
                else
                {
                    LocationPoint lp = _mapper.Map<LocationPoint>(locationPoint);
                    _context.LocationPoints.Add(lp);
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {

                        return BadRequest(e.InnerException.Message);
                    }


                    return Ok();//CreatedAtAction("GetLocationPoint", new { id = locationPoint.Id }, locationPoint);
                }
            }
            else
            {
                return Unauthorized();
            }
        }
        /// <summary>
        /// Delete a specific Location point by unique id
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Product created</response>
        /// <response code="400">Product has missing/invalid values</response>
        /// <response code="500">Oops! Can't delete your product right now</response>
        // DELETE: api/LocationPoints/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocationPoint(int id)
        {
            if (base.User.Identity.Name != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var locationPoint = await _context.LocationPoints.FindAsync(id);
                if (locationPoint == null)
                {
                    return NotFound();
                }
                var select_ = _context.WeightPoints.Where(w => w.LocationPointId == id).Count();
                if (select_ > 0)
                {
                    return BadRequest("В данном размещении существуют весовые точки");
                }
                else
                {

                    _context.LocationPoints.Remove(locationPoint);
                    await _context.SaveChangesAsync();

                    return NoContent();
                }
            }
            else
            {
                return Unauthorized();
            }
        }

        private bool LocationPointExists(int id)
        {
            return _context.LocationPoints.Any(e => e.Id == id);
        }
    }
}
