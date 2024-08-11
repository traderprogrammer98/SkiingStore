using SkiingStore.Entities;
using SkiingStore.RequestHelpers;

namespace SkiingStore.Repositories.Interface
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<PagedList<Product>> GetAllAsync(ProductParams productParams);
    }
}
