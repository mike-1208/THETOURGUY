namespace Infrastructure.External.BigGuy;

public class BigGuyProduct
{
    public ProductDetailData ProductDetailData { get; set; } = new ProductDetailData();
    public PriceData Price { get; set; } = new PriceData();
    public List<string> Photos { get; set; } = new List<string>();
}

public class ProductDetailData
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ProductDescription { get; set; } = string.Empty;
    public double AverageStars { get; set; }
    public int Capacity { get; set; }
}

public class PriceData
{
    public decimal Amount { get; set; }
    public decimal AppliedDiscount { get; set; }
}

