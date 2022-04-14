using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportingAPI.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingAPI.Controllers
{
    [SwaggerTag("Категории")]
    [Route("api/[controller]")]
    [ApiController]

    public class CategoriesController : ControllerBase
    {
        private readonly ReportingContext _context;
        private IMapper _mapper;
        public CategoriesController(ReportingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/WeightPlatforms
        [HttpGet]
        
        public async Task<int> GetWeightPlatforms()
        {
            
            var Categories = new List<Category>
            {
                new Category{Text="test"},
            };

            Categories.ForEach(t => _context.Categories.Add(t));
            _context.SaveChanges();

            return 0;
            /*await _context.Categories.ToListAsync();
            return Unauthorized();*/
        }
    }
}
