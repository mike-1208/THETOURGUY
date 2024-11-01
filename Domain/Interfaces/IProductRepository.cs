using Application.DTOs;

namespace Domain.Interfaces;

public interface IProductRepository
{
    public string Supplier { get; }

    public List<ProductDTO> GetProducts(
        int numberOfGuests, 
        string? name, 
        string? destination, 
        decimal? maxPrice, 
        int skipCount,
        int takeCount);

    public int GetTotalCount(
        int numberOfGuests, 
        string? name, 
        string? destination, 
        decimal? maxPrice);
}