using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using products.data;
using products.models.entities;

namespace products.controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public readonly ProductDbContext dbContext;
        public ProductsController(ProductDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await dbContext.products.ToListAsync();
            if (products == null || products.Count == 0)
            {
                return NotFound("Products not found");
            }
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromForm] AddProduct addProduct)
        {
            // Ensure the upload directory exists
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string? imagePath = null;

            // Check if an image file was uploaded
            if (addProduct.Image != null && addProduct.Image.Length > 0)
            {
                // Generate a unique filename for the image
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(addProduct.Image.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the image to the server
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await addProduct.Image.CopyToAsync(fileStream);
                }

                // Store the relative path to the image in the database
                imagePath = "/images/" + uniqueFileName;
            }

            // Create the new product object
            var newProduct = new Product()
            {
                Name = addProduct.Name,
                Catgory = addProduct.Catgory,
                Price = addProduct.Price,
                imageUrl = imagePath // Save the image path to the database
            };

            // Add the product to the database and save changes
            dbContext.products.Add(newProduct);
            await dbContext.SaveChangesAsync();

            // Return the created product
            return Ok(newProduct);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = dbContext.products.Find(id);
            if (product == null)
            {
                return NotFound("Products not found");
            }
            return Ok(product);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> deletePoroduct(Guid id)
        {
            var product = dbContext.products.Find(id);
            if (product == null)
            {
                return NotFound("Product not found");
            }
            dbContext.products.Remove(product);
            await dbContext.SaveChangesAsync();
            return Ok("Deleted successfully");
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateProduct(Guid id, UpdateProducts updateProducts)

        {
            // Ensure the upload directory exists
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string? imagePath = null;

            // Check if an image file was uploaded
            if (updateProducts.Image != null && updateProducts.Image.Length > 0)
            {
                // Generate a unique filename for the image
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(updateProducts.Image.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the image to the server
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await updateProducts.Image.CopyToAsync(fileStream);
                }

                // Store the relative path to the image in the database
                imagePath = "/images/" + uniqueFileName;
            }
            // Find the product by ID
            var product = await dbContext.products.FindAsync(id);

            if (product == null)
            {
                return NotFound("Product not found");
            }

            // Update the product properties
            product.Name = updateProducts.Name;
            product.Catgory = updateProducts.Catgory;
            product.Price = updateProducts.Price;
            product.imageUrl = imagePath;

            // Save the changes to the database
            await dbContext.SaveChangesAsync();

            return Ok("Product updated successfully");
        }

    }
}
