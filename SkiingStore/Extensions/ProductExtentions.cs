using SkiingStore.Entities;

namespace SkiingStore.Extensions
{
    public static class ProductExtentions
    {
        public static IQueryable<Product> Sort(this IQueryable<Product> query, string orderBy)
        {
            query = orderBy switch
            {
                "price" => query.OrderBy(p => p.Price),
                "priceDesc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name)
            };
            return query;
        }
        public static IQueryable<Product> Search(this IQueryable<Product> query, string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return query;
            }
            query = query.Where(p => p.Name.Contains(search.Trim().ToLower()));
            return query;
        }
        public static IQueryable<Product> Filter(this IQueryable<Product> query, string brands, string types)
        {
            var brandList = new List<string>();
            var typeList = new List<string>();
            if (!string.IsNullOrEmpty(brands))
            {

                brandList.AddRange(brands.ToLower().Split(","));
            }
            if (!string.IsNullOrEmpty(types))
            {

                typeList.AddRange(types.ToLower().Split(","));
            }
            query = query.Where(p => brandList.Count == 0 || brandList.Contains(p.Brand));
            query = query.Where(p => typeList.Count == 0 || typeList.Contains(p.Type));
            return query;
        }
    }
}
