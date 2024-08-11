using SkiingStore.Entities;

namespace SkiingStore.Repositories.Interface
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> GetAllAsync(string orderBy, string search, string brands, string types);
    }
}
