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
    [Authorize(Roles = @"EUROPE\KRR-LG_Inet_Users")]
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
        [AllowAnonymous]
        [HttpGet]
        public async Task<List<Category>> GetCategories()
       // public async Task<ActionResult> GetCategories() 
        {
            return await _context.Categories.ToListAsync();
           // return BadRequest("tst bad req mess");
        }

        // PUT: api/PutCategory
        [HttpPut]
        public async Task<ActionResult> PutCategory(UpdateCategory categoryData)
        {
            Category category = await _context.Categories.FindAsync(categoryData.Id);

            if (category == null)
                return BadRequest("Категории с указанным id не существует");
            category.Text = categoryData.Text;
            category.Description = categoryData.Description;
            category.Visible = categoryData.Visible;

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

        // POST: api/Categories
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

            return Ok(category.Id);
        }

        // PUT: api/PutParentId (перемещение категории)
        [HttpPut("UpdateCategoryParent")]
        //public async Task<ActionResult> PutParentId(int id, [FromBody] int? toCategory)
        public async Task<ActionResult> PutParentId(UpdateCategoryParent CategoryParent = null)
        {

            Category category = await _context.Categories.FindAsync(CategoryParent.id);
            if (category == null)
                return BadRequest("Категории с указанным id не существует");
            CategoryParent.toCat = CategoryParent.toCat == 0 ? null : CategoryParent.toCat;
            category.ParentId = CategoryParent.toCat;

            try
            {              
                _context.Categories.Update(category);
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
            Category category = _context.Categories.Include(x => x.Reports).FirstOrDefault(x => x.Id == id);

            var HasCategory = category.Categories.Any();
            var HasReport = category.Reports.Any();

            List<string> ExceptionTextHasChildren = new List<string>() { "Удаление невозможно, элемент имеет дочерние элементы!",
                "Рекомендации: удалите все дочерние элементы." };

            if (HasReport || HasCategory)
            {
                return BadRequest(ExceptionTextHasChildren);
            }

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
