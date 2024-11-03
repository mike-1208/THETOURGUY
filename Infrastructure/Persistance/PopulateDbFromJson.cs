using System;
using System.Text.Json;
using Domain.Entities;

namespace Infrastructure.Persistance;

public class Image
{
    public string Url { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}

public class Adventure
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double AverageRating { get; set; }
    public decimal RegularPrice { get; set; }
    public decimal DiscountPrice { get; set; }
    public int MaximumGuests { get; set; }
    public List<Image> Images { get; set; } = new List<Image>();
}

public class Root
{
    public List<Adventure> Data { get; set; } = new List<Adventure>();
}

public static class PopulateDbFromJson
{
    const string JsonPath = "..\\Data\\TheTourGuyData.json";
    
    public static void Populate(TourGuyDbContext context)
    {
        try {
            var json = File.ReadAllText(JsonPath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var products = JsonSerializer.Deserialize<Root>(json, options);
            if (products == null)
            {
                Console.WriteLine("No products found in the JSON file");
                return;
            }
            foreach (var product in products.Data)
            {
                var productEntity = new Product
                {
                    Id = product.Id,
                    Title = product.Title,
                    Description = product.Description,
                    AverageRating = product.AverageRating,
                    RegularPrice = product.RegularPrice,
                    DiscountPrice = product.DiscountPrice,
                    MaximumGuests = product.MaximumGuests,
                    Images = product.Images.Select(image => new ProductImage
                    {
                        Url = image.Url,
                        DisplayOrder = image.DisplayOrder
                    }).ToList()
                };
                context.Products.Add(productEntity);
            }
            context.SaveChanges();
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
