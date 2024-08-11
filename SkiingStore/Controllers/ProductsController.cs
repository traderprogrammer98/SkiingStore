using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkiingStore.Data;
using SkiingStore.Extensions;
using SkiingStore.Repositories.Interface;
using SkiingStore.RequestHelpers;
using System.Text.Json;

namespace SkiingStore.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IProductRepository _productRepository;
        private readonly AppDbContext _dbContext;

        public ProductsController(IProductRepository productRepository, AppDbContext dbContext)
        {
            _productRepository = productRepository;
            _dbContext = dbContext;
        }
        [HttpGet()]

        public async Task<IActionResult> GetProducts([FromQuery]ProductParams productParams)
        {
            var products = await _productRepository.GetAllAsync(productParams);
            Response.AddPaginationHeader(products.MetaData);
            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id) 
        {
            var product = await _productRepository.GetAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
               
            return Ok(product);
        }
        [HttpGet("filters")]
        public async Task<IActionResult> GetFilters()
        {
            var brands = await _dbContext.Products.Select(p => p.Brand).Distinct().ToListAsync();
            var types = await _dbContext.Products.Select(p => p.Type).Distinct().ToListAsync();

            return Ok(new { brands, types });
        }
    }
}
