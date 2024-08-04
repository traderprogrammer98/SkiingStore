using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkiingStore.Repositories.Interface;

namespace SkiingStore.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IProductRepository _productRepository;
        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet()]

        public async Task<IActionResult> GetProducts()
        {
            return Ok(await _productRepository.GetAllAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id) 
        {
            return Ok(await _productRepository.GetAsync(p => p.Id == id));
        }
    }
}
