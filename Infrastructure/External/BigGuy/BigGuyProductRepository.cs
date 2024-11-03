using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Interfaces;

namespace Infrastructure.External.BigGuy
{
    // Wrapper class for JSON deserialization
    public class ProductDataWrapper
    {
        public List<BigGuyProduct> ProductData { get; set; } = new List<BigGuyProduct>();
    }

    // The main repository class implementing IProductRepository
    public class BigGuyProductRepository : IProductRepository
    {
        private static readonly string JsonFilePath = "..\\Data\\TheBigGuy.json";
        private List<BigGuyProduct> _products = new List<BigGuyProduct>();

        public string Supplier => "BigGuy";

        public BigGuyProductRepository()
        {
            // Load products from JSON file during instantiation
            _products = LoadProductsAsync().Result;
        }

        // Async method to load products from the JSON file
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

        // Method to get paged products
        private List<BigGuyProduct> GetPagedProducts(
            int numberOfGuests,
            string? name,
            decimal? maxPrice,
            int pageSize,
            int pageIndex)
        {
            var filteredProducts = _products
                .Where(product =>
                    product.ProductDetailData.Capacity >= numberOfGuests &&
                    (name == null || product.ProductDetailData.Name.Contains(name, StringComparison.OrdinalIgnoreCase)) &&
                    (maxPrice == null || (product.Price.Amount * (1 - product.Price.AppliedDiscount)) <= maxPrice))
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToList();

            return filteredProducts;
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

            // Map BigGuyProduct to ProductDTO
            return pagedProducts.Select(p => new ProductDTO
            {
                Name = p.ProductDetailData.Name,
                Description = p.ProductDetailData.ProductDescription,
                Destination = location ?? "Unknown",
                Price = p.Price.Amount * (1 - p.Price.AppliedDiscount),
                Supplier = Supplier
            }).ToList();
        }

        // Method to get the total count of products that match the criteria
        public int GetTotalCount(
            int numberOfGuests,
            string? location,
            string? name,
            decimal? maxPrice)
        {
            return _products.Count(product =>
                product.ProductDetailData.Capacity >= numberOfGuests &&
                (name == null || product.ProductDetailData.Name.Contains(name, StringComparison.OrdinalIgnoreCase)) &&
                (maxPrice == null || (product.Price.Amount * (1 - product.Price.AppliedDiscount)) <= maxPrice));
        }
    }
}
