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

        public async Task<ActionResult<IEnumerable<Category>>> GetWeightPlatforms()
        {

            /*var Categories = new List<Category>
            {
                new Category{Text="test2", ParentId=1},
            };

            Categories.ForEach(t => _context.Categories.Add(t));
            _context.SaveChanges();*/
            /*_context.Categories.Where(x => x.ParentId == null);*/
            var temp = await _context.Categories.Include(x => x.Children).ToListAsync();
            var result = temp.Where(x => x.Parent == null).ToList();

            return result;

            /*var tst =  _context.Categories.ToListAsync();
            var tst2 = _context.Reports.ToListAsync();

            

            return await _context.Categories.ToListAsync();*/
            /*await _context.Categories.ToListAsync();
            return Unauthorized();*/
        }

        // GET: api/StaticGet
        /*[HttpGet]
        public ActionResult<IEnumerable<string>> StaticGet()
        {
            string json = @"
[
    {
        ""id"": 1,
        ""text"": ""Root node"",
        ""type"": ""folder"",
        ""children"": [
            {
                ""id"": 2,
                ""text"": ""Child node 1"",
                ""type"": ""file"",
                ""data"": {
                    ""url"": ""test data2""
                }
            },
            {
                ""id"": 3,
                ""text"": ""Child node 2"",
                ""type"": ""folder"",
                ""children"": [
                    {
                        ""id"": 6,
                        ""text"": ""test child"",
                        ""type"": ""folder""
                    }
                ]
            },
            {
                ""id"": 4,
                ""text"": ""Test node 3"",
                ""type"": ""file""
            }
        ]
    },
    {
        ""id"": 5,
        ""text"": ""root 2"",
        ""type"": ""folder""
    }
]";
            return new string[] { "te", "asd" };
        }*/
        [HttpGet]
        public bool GetT()
        {
            return false;
        }
    }
}
