using System.Text.Json;
using Domain.Interfaces;
using Application.DTOs;

namespace Infrastructure.External.SomeOtherGuy;

public class ProductDataWrapper
{
    public List<SomeOtherGuyProduct> Products { get; set; } = new List<SomeOtherGuyProduct>();
}


public class SomeOtherGuyProductRepository : IProductRepository
{
    private List<SomeOtherGuyProduct> _products = new List<SomeOtherGuyProduct>();
    private const string JsonFilePath = "..\\Data\\SomeOtherGuyData.json";
    
    public SomeOtherGuyProductRepository()
    {
        _products = LoadProductsAsync().Result;
    }

    private async Task<List<SomeOtherGuyProduct>> LoadProductsAsync()
    {
        try
        {
            var json = await File.ReadAllTextAsync(JsonFilePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var wrapper = JsonSerializer.Deserialize<ProductDataWrapper>(json, options);
            return wrapper?.Products ?? new List<SomeOtherGuyProduct>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading products: {ex.Message}");
            return new List<SomeOtherGuyProduct>();
        }
    }

    // asume that this is the external api
    public List<SomeOtherGuyProduct> GetPagedProducts(
        int numberOfGuests, 
        string? name, 
        decimal? maxPrice, 
        int pageSize, 
        int pageIndex
    ){
        var filteredProducts = _products
        .Where(p => 
            p.Capacity >= numberOfGuests && 
            (name == null || p.Name.ToLower().Contains(name.ToLower())) && 
            (maxPrice == null || (p.Price * (1 - p.DiscountPercentage / 100)) <= maxPrice));

        return filteredProducts.Skip(pageSize * pageIndex).Take(pageSize).ToList();
    }

    // asume that this is the external api
    public int GetTotalCount(
        int numberOfGuests, 
        string? location,
        string? name, 
        decimal? maxPrice)
    {
        return _products.Count(p => 
            p.Capacity >= numberOfGuests && 
            (name == null || p.Name.ToLower().Contains(name.ToLower())) && 
            (maxPrice == null || (p.Price * (1 - p.DiscountPercentage / 100)) <= maxPrice));
    }

    public List<ProductDTO> GetProducts(
        int numberOfGuests,
        string? location,
        string? name,
        decimal? maxPrice,
        int pageSize,
        int pageIndex)
    {
        var products = GetPagedProducts(numberOfGuests, name, maxPrice, pageSize, pageIndex);
        return products.Select(p => new ProductDTO
        {

        }).ToList();
    }

    public string Supplier => "SomeOtherGuy";
}