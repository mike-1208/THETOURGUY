using Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService
    {
        private readonly ProductRepositoryAggregator _repositoryAggregator;

        public ProductService(ProductRepositoryAggregator repositoryAggregator)
        {
            _repositoryAggregator = repositoryAggregator;
        }

        public List<ProductDTO> GetProducts(
            int numberOfGuests,
            string? name = null,
            string? destination = null,
            string? supplier = null,
            decimal? maxPrice = null,
            int pageSize = 10,
            int pageIndex = 0)
        {
            // Use the aggregator to fetch products with specified filters
            return _repositoryAggregator.GetProducts(
                numberOfGuests, name, destination, supplier, maxPrice, pageSize, pageIndex);
        }

        public int GetTotalProductCount(
            int numberOfGuests,
            string? name = null,
            string? destination = null,
            string? supplier = null,
            decimal? maxPrice = null)
        {
            // Use the aggregator to get the total count of products matching the filters
            return _repositoryAggregator.GetTotalCount(
                numberOfGuests, name, destination, supplier, maxPrice);
        }
    }
}
