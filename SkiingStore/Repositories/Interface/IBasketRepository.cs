using SkiingStore.Entities;

namespace SkiingStore.Repositories.Interface
{
    public interface IBasketRepository
    {
        Task<Basket> GetBasketAsync();
        Task<Basket> AddBasketAsync();
    }
}
