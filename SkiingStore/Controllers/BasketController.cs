using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkiingStore.Data;
using SkiingStore.DTOs;
using SkiingStore.Entities;
using SkiingStore.Repositories.Interface;

namespace SkiingStore.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly AppDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(AppDbContext context,IProductRepository productRepository,IBasketRepository basketRepository, IMapper mapper)
        {
            _context = context;
            _productRepository = productRepository;
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        [HttpGet(Name = "GetBasket")]
        public async Task<ActionResult<BasketDto>> GetBasket()
        {
            var basket = await _basketRepository.GetBasketAsync();
            if (basket == null)
            {
                return NotFound();
            }
            var basketDto = _mapper.Map<BasketDto>(basket);
            return basketDto;
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
            var basketDto = _mapper.Map<BasketDto>(basket);
            if (result) return CreatedAtRoute(nameof(GetBasket), basketDto);
            return BadRequest(new ProblemDetails { Title = "Problem saving item to basket."});
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveBasketItem(int productId, int quantity)
        {
            var basket = await _basketRepository.GetBasketAsync();
            if (basket == null)
            {
                return NotFound();
            }
            var product = await _productRepository.GetAsync(p =>p.Id == productId);
            if (product == null)
            {
                return NotFound();
            }
            basket.RemoveItem(product, quantity);
            var result = await _context.SaveChangesAsync() > 0;
            if (result)
            {
                var basketDto = _mapper.Map<BasketDto>(basket);
                return CreatedAtRoute(nameof(GetBasket), basketDto);
            }
            return BadRequest(new ProblemDetails { Title = "item did not remove" });
        }

    }
}
