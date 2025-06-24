using Microsoft.EntityFrameworkCore;
using WebElectroShop.Server.Core.DTO;

namespace WebElectroShop.Server.Core.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var totalItems = await query.CountAsync(cancellationToken);

            var items = await query.Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync(cancellationToken);

            return new PagedResult<T>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems
            };
        }
    }
}
