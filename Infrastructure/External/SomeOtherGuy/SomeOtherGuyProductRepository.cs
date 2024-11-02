using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Interfaces;
using Application.DTOs;

namespace Infrastructure.External.SomeOtherGuy
{
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
            // Load products from JSON file during instantiation
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

        // Simulated external API to get paged products
        public List<SomeOtherGuyProduct> GetPagedProducts(
            int numberOfGuests, 
            string? name, 
            decimal? maxPrice, 
            int pageSize, 
            int pageIndex)
        {
            var filteredProducts = _products
                .Where(p => 
                    p.Capacity >= numberOfGuests && 
                    (name == null || p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)) && 
                    (maxPrice == null || (p.Price * (1 - p.DiscountPercentage / 100)) <= maxPrice));

            return filteredProducts
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToList();
        }

        // Simulated external API to get total count of products
        public int GetTotalCount(
            int numberOfGuests, 
            string? location,
            string? name, 
            decimal? maxPrice)
        {
            return _products.Count(p => 
                p.Capacity >= numberOfGuests && 
                (name == null || p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)) && 
                (maxPrice == null || (p.Price * (1 - p.DiscountPercentage / 100)) <= maxPrice));
        }

        // Main method to retrieve products as ProductDTO
        public List<ProductDTO> GetProducts(
            int numberOfGuests,
            string? location,
            string? name,
            decimal? maxPrice,
            int pageSize,
            int pageIndex)
        {
            // Use GetPagedProducts to get filtered products
            var pagedProducts = GetPagedProducts(numberOfGuests, name, maxPrice, pageSize, pageIndex);

            // Map SomeOtherGuyProduct to ProductDTO
            return pagedProducts.Select(p => new ProductDTO
            {
                Name = p.Name,
                Description = p.ProductDescription,
                DestinationName = location ?? "Unknown",
                Price = p.Price * (1 - p.DiscountPercentage / 100),
                SupplierName = Supplier
            }).ToList();
        }

        public string Supplier => "SomeOtherGuy";
    }
}
