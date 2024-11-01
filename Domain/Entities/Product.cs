namespace Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double AverageRating { get; set; }
    public decimal RegularPrice { get; set; }
    public decimal DiscountPrice { get; set; }
    public int MaximumGuests { get; set; }

    // Navigation Property
    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();


}

