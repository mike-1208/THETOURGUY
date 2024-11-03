using System.Collections.Generic;
using System.Linq;
using Application.DTOs;
using Domain.Interfaces;
using Infrastructure.Persistance;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public string Supplier { get; } = "Internal";
        private readonly TourGuyDbContext _context;

        public ProductRepository(TourGuyDbContext context)
        {
            _context = context;
        }

        public List<ProductDTO> GetProducts(
            int numberOfGuests, 
            string? name, 
            string? destination, 
            decimal? maxPrice, 
            int skipCount, 
            int takeCount)
        {
            var query = _context.Products
                .Where(p => p.MaximumGuests >= numberOfGuests)
                .Where(p => string.IsNullOrEmpty(name) || p.Title.Contains(name))
                .Where(p => !maxPrice.HasValue || p.DiscountPrice <= maxPrice);

            return query.Skip(skipCount)
                .Take(takeCount)
                .Select(p => new ProductDTO
                {
                    Name = p.Title,  // Corrected to use Title instead of Name
                    Description = p.Description,
                    Destination = destination ?? "Unknown",
                    Price = p.DiscountPrice,
                    Supplier = Supplier
                }).ToList();
        }

        public int GetTotalCount(
            int numberOfGuests, 
            string? name, 
            string? destination, 
            decimal? maxPrice)
        {
            var query = _context.Products
                .Where(p => p.MaximumGuests >= numberOfGuests)
                .Where(p => string.IsNullOrEmpty(name) || p.Title.Contains(name))  // Corrected to use Title instead of Name
                .Where(p => !maxPrice.HasValue || p.DiscountPrice <= maxPrice);

            return query.Count();
        }
    }
}
