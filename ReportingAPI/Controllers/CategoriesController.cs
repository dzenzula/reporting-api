using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        [AllowAnonymous]
        // GET: api/Categories
        [HttpGet]
        public async Task<List<Category>> GetCategories()
       // public async Task<ActionResult> GetCategories() 
        {
            /*var temp = await _context.Categories.Include(x => x.Reports).ToListAsync();
             var result = temp.Where(x => x.ParentId == null).ToList();*/

            return await _context.Categories.ToListAsync();
           // return BadRequest("tst bad req mess");
        }

        // PUT: api/PutCategory
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<ActionResult> PutCategory(UpdateCategory category)
        {
            /*category.ParentId = null;
            category.Id = 7;
            category.Text = "test";*/ // Origin: test

            /*List<UpdateCategory> tst = new List<UpdateCategory> { };
            tst.i*/
            
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
        [Authorize(Roles = @"EUROPE\KRR-LG_Inet_Users")]
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

        // PUT: api/PutParentId (updating parentId)
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}, {parentId}")]
        public async Task<ActionResult> PutParentId(int id, [FromBody] int? parentId)
        {
            var category = await _context.Categories.FindAsync(id);

            parentId = parentId == 0 ? null : parentId;

            category.ParentId = parentId;
            _context.Categories.Update(category);

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

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var HasCategory = _context.Categories.Any(x => id == x.ParentId);
            var HasReport = _context.Reports.Any(x => id == x.ParentId);
            List<string> ExceptionTextHasChildren = new List<string>() { "Удаление невозможно, элемент имеет дочерние элементы!",
                "Рекомендации: удалите все дочерние элементы." };

            if (HasReport || HasCategory)
            {
                return BadRequest(ExceptionTextHasChildren);
            }
            var category = await _context.Categories.FindAsync(id);
            try
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.InnerException.Message);
            }
        }
    }
}
