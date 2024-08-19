using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using products.data;
using products.models.entities;

namespace products.controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatgoryController : ControllerBase
    {
        public readonly ProductDbContext dbContext;
        public CatgoryController(ProductDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetCatgories()
        {
            var categories = await dbContext.categories.ToListAsync();
            if (categories == null || categories.Count == 0)
            {
                return NotFound("catories not found");
            }
            return Ok(categories);
        }


        [HttpPost]
        public async Task<IActionResult> AddCatgory(AddCatgory addCatgory)
        {
            var categories = await dbContext.categories.ToListAsync();
            var newCatgory = new Category()
            {
                Name = addCatgory.Name
            };
            dbContext.categories.Add(newCatgory);
            await dbContext.SaveChangesAsync();
            return Ok(newCatgory);

        }


        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeleteCatgory(Guid id)
        {
            var category = dbContext.categories.Find(id);

            if (category == null)
            {
                return NotFound("category not found");
            }

            dbContext.categories.Remove(category);
            await dbContext.SaveChangesAsync();
            return Ok("deleted successfully");

        }

        [HttpPut]
        [Route("{id:guid}")]

        public async Task<IActionResult> UpdateCatgory(Guid id, UpdateCatgory updateCatgory)
        {

            var category = dbContext.categories.Find(id);
            if (category == null)
            {
                return NotFound("category not found");
            }

            category.Name = updateCatgory.Name;
            await dbContext.SaveChangesAsync();
            return Ok("updated successfully");
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetCatgoryById(Guid id)
        {
            var category = dbContext.categories.Find(id);
            if (category == null)
            {
                return NotFound("category not found");
            }
            return Ok(category);
        }
    }
}
