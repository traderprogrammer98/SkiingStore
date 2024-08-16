using Microsoft.EntityFrameworkCore;
using SkiingStore.Data;
using SkiingStore.Entities.OrderAggregate;
using SkiingStore.Repositories.Interface;

namespace SkiingStore.Repositories.Implementation
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderRepository(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            var order = await _dbContext.Orders.Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.BuyerId == _httpContextAccessor.HttpContext.User.Identity.Name && o.Id == id);
            return order;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            var orders = await _dbContext.Orders.Include(o => o.OrderItems)
                .Where(o => o.BuyerId == _httpContextAccessor.HttpContext.User.Identity.Name)
                .ToListAsync();
            return orders;
        }
    }
}
