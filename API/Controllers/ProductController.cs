using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Application.DTOs;
using Domain.Entities;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ProductController : ControllerBase
{
    private readonly ProductRepositoryAggregator _productRepositoryAggregator;

    public ProductController(ProductRepositoryAggregator productRepositoryAggregator)
    {
        _productRepositoryAggregator = productRepositoryAggregator;
    }

    [HttpGet("products")]
    public IActionResult GetProducts(
        int numberOfGuests, 
        string? name, 
        string? destination, 
        string? supplier, 
        decimal? maxPrice, 
        int pageSize = 10, 
        int pageIndex = 0)
    {
        return Ok(_productRepositoryAggregator
            .GetProducts(
                numberOfGuests, name, destination, 
                supplier, maxPrice, pageSize, pageIndex));
    }

}