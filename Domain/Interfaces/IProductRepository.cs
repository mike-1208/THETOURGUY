using Application.DTO;

namespace Domain.Interfaces;

public interface IProductrepository;
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