namespace Application.DTOs;
public class  ProductDTO
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Supplier { get; set; } = string.Empty;
}