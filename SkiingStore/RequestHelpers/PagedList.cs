using Microsoft.EntityFrameworkCore;

namespace SkiingStore.RequestHelpers
{
    public class PagedList<T> : List<T> where T : class
    {
        public MetaData MetaData;
        public PagedList(List<T> items, int pageNumber, int totalPages, int pageSize, int count)
        {
            MetaData = new MetaData
            {
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalCount = count
            };
            AddRange(items);
        }
        public static async Task<PagedList<T>> ToPagedList(IQueryable<T> query, int pageNumber, int pageSize)
        {
            var count = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, pageNumber, totalPages, pageSize, count);
        }
    }
}
