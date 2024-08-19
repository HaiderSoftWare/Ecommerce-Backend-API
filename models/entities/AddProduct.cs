using System;

namespace products.models.entities;

public class AddProduct
{

    public string Name { get; set; } = null!;

    public string Catgory { get; set; } = null!;

    public decimal Price { get; set; }
    public IFormFile? Image { get; set; } // For the image upload


}
