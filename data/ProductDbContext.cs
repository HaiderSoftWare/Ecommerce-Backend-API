using System;
using System.Data;
using Microsoft.EntityFrameworkCore;
using products.models.entities;

namespace products.data;

public class ProductDbContext : DbContext
{

    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }
    public DbSet<Product> products { get; set; }
    public DbSet<Category> categories { get; set; }

}
