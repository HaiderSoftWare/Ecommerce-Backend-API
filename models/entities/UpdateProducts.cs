using System;

namespace products.models.entities;

public class UpdateProducts
{


    public string Name { get; set; } = null!;

    public string Catgory { get; set; } = null!;

    public decimal Price { get; set; }

    public IFormFile? Image { get; set; } 


}
