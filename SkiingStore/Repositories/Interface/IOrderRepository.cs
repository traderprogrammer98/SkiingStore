using SkiingStore.Entities.OrderAggregate;

namespace SkiingStore.Repositories.Interface
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<List<Order>> GetOrdersAsync();
        Task<Order> GetOrderAsync(int id);
    }
}
