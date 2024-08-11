using Microsoft.EntityFrameworkCore;
using SkiingStore.Data;
using SkiingStore.Entities;
using SkiingStore.Extensions;
using SkiingStore.Repositories.Interface;

namespace SkiingStore.Repositories.Implementation
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
            
        }

        public async Task<List<Product>> GetAllAsync(string orderBy, string search, string brands, string types)
        {
            var query = _context.Products.AsQueryable().Search(search).Sort(orderBy).Filter(brands, types);
            return await query.ToListAsync();
        }
    }
}
