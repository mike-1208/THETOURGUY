using System.Text.Json;
using Domain.Interfaces;
using Application.DTOs;

namespace Infrastructure.External.BigGuy;

public class ProductDataWrapper
{
    public List<BigGuyProduct> ProductData { get; set; } = new List<BigGuyProduct>();
}

public class BigGuyProductRepository : IProductRepository
{
    private static readonly string JsonFilePath = "..\\Data\\TheBigGuy.json";
    private List<BigGuyProduct> _products = new List<BigGuyProduct>();

    public BigGuyProductRepository()
    {
        _products = LoadProductsAsync().Result;
    }

    private async Task<List<BigGuyProduct>> LoadProductsAsync()
    {
        try
        {
            var json = await File.ReadAllTextAsync(JsonFilePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var wrapper = JsonSerializer.Deserialize<ProductDataWrapper>(json, options);
            return wrapper?.ProductData ?? new List<BigGuyProduct>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading products: {ex.Message}");
            return new List<BigGuyProduct>();
        }
    }

    public List<BigGuyProduct> GetPagedProducts(
        int numberOfGuests, 
        string? name, 
        decimal? maxPrice, 
        int pageSize, 
        int pageIndex)
    {
        var filteredProducts = _products
            .Where(product => 
                product.ProductDetailData.Capacity >= numberOfGuests &&
                (name == null || product.ProductDetailData.Name.ToLower().Contains(name.ToLower())) &&
                (maxPrice == null || (product.Price.Amount * (1 - product.Price.AppliedDiscount)) <= maxPrice));
        return filteredProducts.Skip(pageSize * pageIndex).Take(pageSize).ToList();
    }   

    // asume that this is the external api
    public int GetTotalCount(
        int numberOfGuests, 
        string? location,
        string? name, 
        decimal? maxPrice)
    {
        var filteredProducts = _products
            .Where(product => 
                product.ProductDetailData.Capacity >= numberOfGuests &&
                (location == null || true) &&
                (name == null || product.ProductDetailData.Name.ToLower().Contains(name.ToLower())) &&
                (maxPrice == null || (product.Price.Amount * (1 - product.Price.AppliedDiscount)) <= maxPrice));
        return filteredProducts.Count();
    }

    public List<ProductDTO> GetProducts(
        int numberOfGuests,
        string? location,
        string? name,
        decimal? maxPrice,
        int pageSize,
        int pageIndex)
    {
        var filteredProducts = _products
            .Where(product => 
                product.ProductDetailData.Capacity >= numberOfGuests &&
                (location == null || true) &&
                (name == null || product.ProductDetailData.Name.ToLower().Contains(name.ToLower())) &&
                (maxPrice == null || (product.Price.Amount * (1 - product.Price.AppliedDiscount)) <= maxPrice));
        return filteredProducts
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .Select(p => new ProductDTO
            {
                // Map your BigGuyProduct properties to ProductDTO properties here
            })
            .ToList();
    }

    public string Supplier => "BigGuy";
}
