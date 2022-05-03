using AutoMapper;
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

        // GET: api/Categories
        [HttpGet]
        public async Task<List<Category>> GetCategories()
        {
           // var temp = await _context.Categories.Include(x => x.Reports).ToListAsync();
          //  var result = temp.Where(x => x.ParentId == null).ToList();

            return await _context.Categories.ToListAsync();
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


            /*Console.WriteLine(category);
            return Ok();*/
           try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(ex.Message);
            }

            //   var tst = Ok();
           // return Ok();
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostCategory(AddCategory newCategory)
        {
            Category category = _mapper.Map<Category>(newCategory);

            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {

                return BadRequest(e.InnerException.Message);
            }

            //int id = category.Id;
            return Ok(category.Id);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<int> DeleteCategory(int id)
        {
            
            return 100;
        }
            // GET: api/StaticGet
            /*[HttpGet]
            public string StaticGet()
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
                return json;
            }*/
        }
}
