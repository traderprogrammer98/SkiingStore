using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkiingStore.Data;
using SkiingStore.Entities;
using SkiingStore.Repositories.Interface;

namespace SkiingStore.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly AppDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly IBasketRepository _basketRepository;

        public BasketController(AppDbContext context,IProductRepository productRepository,IBasketRepository basketRepository)
        {
            _context = context;
            _productRepository = productRepository;
            _basketRepository = basketRepository;
        }
        [HttpGet]
        public async Task<ActionResult<Basket>> GetBasket()
        {
            var basket = await _basketRepository.GetBasketAsync();
            if (basket == null)
            {
                return NotFound();
            }
            return basket;
        }
        [HttpPost]
        public async Task<IActionResult> AddItemToBasket(int productId, int quantity)
        {
            var basket = await _basketRepository.GetBasketAsync();
            if (basket == null)
            {
                basket = await _basketRepository.AddBasketAsync();
            }
            var product = await _productRepository.GetAsync(p => p.Id == productId);
            if (product == null)
            {
                return NotFound();
            }
            basket.AddItem(product, quantity);
            var result = await _context.SaveChangesAsync() > 0;
            if (result) return StatusCode(201);
            return BadRequest(new ProblemDetails { Title = "Problem saving item to basket."});
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveBasketItem(int productId, int quantity)
        {
            return StatusCode(201);
        }

    }
}
