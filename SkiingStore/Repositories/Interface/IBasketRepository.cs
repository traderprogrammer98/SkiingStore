using SkiingStore.Entities;

namespace SkiingStore.Repositories.Interface
{
    public interface IBasketRepository : IRepository<Basket>
    {
        Task<Basket> GetBasketAsync(string buyerId);
        Task<Basket> AddBasketAsync();
        string GetBuyerId();

    }
}
