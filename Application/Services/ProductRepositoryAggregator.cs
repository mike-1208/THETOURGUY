using Application.DTOs;
using Domain.Interfaces;

namespace Application.Services
{
    public class ProductRepositoryAggregator
    {
        private readonly List<IProductRepository> _repositories;

        public ProductRepositoryAggregator(IEnumerable<IProductRepository> repositories)
        {
            _repositories = repositories.ToList();
        }

        public List<ProductDTO> GetProducts(
            int numberOfGuests,
            string? name,
            string? destination,
            string? supplier,
            decimal? maxPrice,
            int pageSize,
            int pageIndex)
        {
            int skipCount = pageIndex * pageSize;
            int takeCount = pageSize;

            if (!string.IsNullOrEmpty(supplier))
            {
                return _repositories
                    .FirstOrDefault(r => r.Supplier.Equals(supplier, StringComparison.OrdinalIgnoreCase))?
                    .GetProducts(numberOfGuests, name, destination, maxPrice, skipCount, takeCount) ?? 
                    new List<ProductDTO>();
            }

            var products = new List<ProductDTO>();
            foreach (var repository in _repositories)
            {
                if (takeCount <= 0) break;

                var rawProducts = repository.GetProducts(numberOfGuests, name, destination, maxPrice, skipCount, takeCount);
                takeCount -= rawProducts.Count;
                skipCount -= repository.GetTotalCount(numberOfGuests, name, destination, maxPrice);
                products.AddRange(rawProducts);
            }
            return products;
        }

        public int GetTotalCount(
            int numberOfGuests,
            string? name,
            string? destination,
            string? supplier,
            decimal? maxPrice)
        {
            if (!string.IsNullOrEmpty(supplier))
            {
                return _repositories
                    .FirstOrDefault(r => r.Supplier.Equals(supplier, StringComparison.OrdinalIgnoreCase))?
                    .GetTotalCount(numberOfGuests, name, destination, maxPrice) ?? 0;
            }

            return _repositories.Sum(r => r.GetTotalCount(numberOfGuests, name, destination, maxPrice));
        }

        public void AddRepository(IProductRepository repository)
        {
            if (!_repositories.Contains(repository))
            {
                _repositories.Add(repository);
            }
        }
    }
}
