public class SomeOtherGuyProduct
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ProductDescription { get; set; } = string.Empty;
    public RatingStatistics RatingStatistics { get; set; } = new RatingStatistics();
    public decimal Price { get; set; }
    public decimal DiscountPercentage { get; set; }
    public int Capacity { get; set; }
    public List<string> ImageUrls { get; set; } = new List<string>();
}

public class RatingStatistics
{
    public int TotalNumberOfReviews { get; set; }
    public int TotalRating { get; set; }
}