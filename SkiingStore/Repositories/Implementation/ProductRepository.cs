using SkiingStore.Data;
using SkiingStore.Entities;
using SkiingStore.Extensions;
using SkiingStore.Repositories.Interface;
using SkiingStore.RequestHelpers;

namespace SkiingStore.Repositories.Implementation
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
            
        }

        public async Task<PagedList<Product>> GetAllAsync(ProductParams productParams)
        {
            var query = _context.Products.Search(productParams.Search).Sort(productParams.SortBy).Filter(productParams.Brands, productParams.Types);
            var products = await PagedList<Product>.ToPagedList(query, productParams.PageNumber, productParams.PageSize);
            return products;
        }
    }
}
