using APIStart.DTOs.ProductDtos;
using APIStart.Models;
using APIStart.Repositories.Implementations;
using APIStart.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIStart.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductsController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productRepository.GetAll().ToListAsync();

        List<ProductGetDto> productGetDtos = new(); // Dto type`da List yaradırıq və List olaraq götürdüyümüz Product`ları loop daxilində List`ə add edirik
        foreach (var product in products)
        {
            productGetDtos.Add(new ProductGetDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Discount = product.DiscountPercent,
                Description = product.Description,
                Rating = product.Rating,
                IsInStock = product.IsInStock,
            });
        }
        return Ok(productGetDtos);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductPostDto productPostDto) //Body`dən Dto` qəbul edirik
    {
        Product product = new()
        {
            Name = productPostDto.Name,
            Price = productPostDto.Price,
            DiscountPercent = productPostDto.Discount,
            Description = productPostDto.Description,
            Rating = productPostDto.Rating,
            IsInStock = productPostDto.IsInStock,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            IsDeleted = false
        };
        await _productRepository.CreateAsync(product); // Repository daxilində olan methodlardan istifadə edirik
        await _productRepository.SaveAsync();

        return StatusCode(StatusCodes.Status201Created, "Product successfully created");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null)
            return NotFound($"Product not found by id: {id}");

        return Ok(product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProductPutDto productPutDto)
    {
        var product = await _productRepository.GetByIdAsync(id); // Repository daxilində olan GetByIdAsync methodundan istifadə edirik
        if (product is null)
            return NotFound($"Product not found by id: {id}");

        if (product.Id != productPutDto.Id)
            return BadRequest();

        product.Name = productPutDto.Name;
        product.Price = productPutDto.Price;
        product.DiscountPercent = productPutDto.Discount;
        product.Description = productPutDto.Description;
        product.Rating = productPutDto.Rating;
        product.IsInStock = productPutDto.IsInStock;
        product.ModifiedAt = DateTime.UtcNow;

        _productRepository.Update(product); // Repository daxilində olan Update methodundan istifadə edirik
        await _productRepository.SaveAsync(); // Repository daxilində olan SaveAsync methodundan istifadə edirik

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productRepository.GetByIdAsync(id); // Repository daxilində olan GetByIdAsync methodundan istifadə edirik Route`dan gələn İd`ni methoda göndəririk
        if (product is null)
            return NotFound($"Product not found by id: {id}");

        _productRepository.Delete(product);
        await _productRepository.SaveAsync();

        return StatusCode(StatusCodes.Status200OK, "Product successfully deleted");
    }
}
