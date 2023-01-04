using AuthorizationApiHandler.Context;
using AuthorizationApiHandler;
using AuthorizationApiHandler.PolicysAuthorize;
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
using Microsoft.AspNetCore.Http;

namespace ReportingApi.Controllers
{
    [SwaggerTag("Категории")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ReportingContext _context;
        private IMapper _mapper;
        private readonly AuthContext _authContext;
        IHttpContextAccessor _httpContextAccessor = null;

        public CategoriesController(ReportingContext context, IMapper mapper, AuthContext authContext, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _authContext = authContext;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/Categories/GetCategoriesForAdmin
        [MultiplePolicysAuthorize("access_to_admin_page")]
        [HttpGet("GetCategoriesForAdmin")]
        public async Task<List<Category>> GetCategoriesAdmin()
        {
            return await _context.Categories.ToListAsync();
        }

        // GET: api/Categories
        // [AllowAnonymous]
        [HttpGet]
        public async Task<List<Category>> GetCategoriesMain()
        {
            const string PUBLIC_REPORT_OPERATION = "public";
            AuthorizeHelper auth = new AuthorizeHelper(_httpContextAccessor, _authContext);
            List<string> MyPermissions = auth.Init().GetAllowedPermissions();
            bool IsAdmin = MyPermissions.Contains(Startup.ADMIN_OPERATION_NAME);
            // Выборка категорий в котрых есть отчеты
            List<Category> categories = await _context.Categories.Include(x => x.Reports)
                .Where(x => x.Reports
                .Where(y =>
                (IsAdmin || y.Operation_name == PUBLIC_REPORT_OPERATION
                || MyPermissions.Contains(y.Operation_name))
                && y.Visible == true)
                .Count() > 0 && x.Visible == true)
                .ToListAsync();

            // Добавление родительских категорий
            List<Category> allCategories = categories.ToList();
            foreach (Category category in categories)
            {
                var isCategoryListed = allCategories.Any(x => category.ParentId == x.Id || category.ParentId == null);
                if (!isCategoryListed)
                {
                    Category parentCategory = _context.Categories.First(x => x.Id == category.ParentId);
                    allCategories.Add(parentCategory);
                }
            }

            return allCategories;
        }
        // PUT: api/PutCategory
        [MultiplePolicysAuthorize("access_to_admin_page")]
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
        [MultiplePolicysAuthorize("access_to_admin_page")]
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
        [MultiplePolicysAuthorize("access_to_admin_page")]
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
        [MultiplePolicysAuthorize("access_to_admin_page")]
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
