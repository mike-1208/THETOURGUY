using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;
public class ProductImage
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public int ProductId { get; set; }    
    [ForeignKey("ProductId")]
    public Product? Product { get; set; }
}
