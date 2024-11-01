namespace Application.DTOs;
public class  ProductDTO
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DestinationName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string SupplierName { get; set; } = string.Empty;
}