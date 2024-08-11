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

        public async Task<IActionResult> GetProducts([FromQuery]string orderBy, [FromQuery]string search, [FromQuery]string brands, [FromQuery]string types)
        {
            return Ok(await _productRepository.GetAllAsync(orderBy, search, brands, types));
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
    }
}
