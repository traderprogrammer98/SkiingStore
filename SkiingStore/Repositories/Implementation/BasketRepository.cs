using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SkiingStore.Data;
using SkiingStore.Entities;
using SkiingStore.Repositories.Interface;

namespace SkiingStore.Repositories.Implementation
{
    public class BasketRepository : IBasketRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;

        public BasketRepository(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor, ILogger<IBasketRepository> logger)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        public async Task<Basket> GetBasketAsync()
        {
            _logger.LogInformation(_httpContextAccessor.HttpContext.Request.Cookies["buyerId"]);
            var basket = await _dbContext.Baskets.Include(b => b.items).ThenInclude(i => i.Product).FirstOrDefaultAsync(b => b.BuyerId == _httpContextAccessor.HttpContext.Request.Cookies["buyerId"]);
            return basket;
        }

        public async Task<Basket> AddBasketAsync()
        {
            var buyerId = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions { IsEssential = true, Expires = DateTime.Now.AddDays(10), SameSite = SameSiteMode.None, Secure = true };
            _httpContextAccessor.HttpContext.Response.Cookies.Append("buyerId", buyerId, cookieOptions);
            var basket = new Basket { BuyerId = buyerId };
            await _dbContext.Baskets.AddAsync(basket);
            await _dbContext.SaveChangesAsync();
            return basket;
        }
    }
}
