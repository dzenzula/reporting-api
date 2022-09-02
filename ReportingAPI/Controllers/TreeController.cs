using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportingApi.Dtos;
using ReportingApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingApi.Controllers
{
    [SwaggerTag("Каталог")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TreeController : ControllerBase
    {
        private readonly ReportingContext _context;
        private IMapper _mapper;

        public TreeController(ReportingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // PUT: api/PutCategory
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<ActionResult> PutCategory(UpdateCategory category)
        {
            /*category.ParentId = null;
            category.Id = 7;
            category.Text = "test";*/ // Origin: test

            Category Categories = _mapper.Map<Category>(category);
            _context.Entry(Categories).State = EntityState.Modified;

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
}
