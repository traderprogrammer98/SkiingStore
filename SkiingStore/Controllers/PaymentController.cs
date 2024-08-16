using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkiingStore.Data;
using SkiingStore.DTOs;
using SkiingStore.Repositories.Interface;
using SkiingStore.Services;

namespace SkiingStore.Controllers
{

    public class PaymentController : BaseApiController
    {
        private readonly PaymentService _paymentService;
        private readonly IBasketRepository _basketRepository;
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public PaymentController(PaymentService paymentService, IBasketRepository basketRepository, AppDbContext dbContext, IMapper mapper)
        {
            _paymentService = paymentService;
            _basketRepository = basketRepository;
            _dbContext = dbContext;
            _mapper = mapper;
        }
        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> CreateOrUpdatePaymentIntent()
        {
            var basket = await _basketRepository.GetBasketAsync(User.Identity.Name);
            if (basket == null)
            {
                return NotFound();
            }
            var intent = await _paymentService.CreateOrUpdatePaymentIntent(basket);
            if (intent == null) 
            {
                return BadRequest(new ProblemDetails { Title = "Problem creating new intent" });
            }
            var result = await _dbContext.SaveChangesAsync() > 0;
            if (!result)
            {
                return BadRequest(new ProblemDetails { Title = "Problem updating basket intent." });
            }
            var basketDto = _mapper.Map<BasketDto>(basket);
            return Ok(basketDto);
        }
    }
}
