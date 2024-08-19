using System;

namespace products.models.entities;

public class Product


{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? imageUrl { get; set; } 
    public required string Catgory { get; set; }
    public decimal Price { get; set; }

}
