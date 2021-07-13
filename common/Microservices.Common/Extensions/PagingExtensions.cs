using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microservices.Common.Types.Paging;

namespace Microservices.Common.Extensions
{
    public static class PagingExtensions
    {
        public static PagedResult<T> GetPagedResult<T>(this IEnumerable<T> items, int currentPage, int rowsPerPage)
        {
            var pagedItems = rowsPerPage < 0 ? items : items.Skip(currentPage * rowsPerPage).Take(rowsPerPage);

            return new PagedResult<T>(pagedItems, items?.Count() ?? 0);
        }

        public static PagedResult<TMapped> GetMappedPagedResult<TOriginal, TMapped>
            (this IQueryable<TOriginal> items, int currentPage, int rowsPerPage, IMapper mapper)
        {
            var pagedItems = (
                    rowsPerPage < 0 ? items : items
                        .Skip(currentPage * rowsPerPage)
                        .Take(rowsPerPage)
                )
                .Select(x => mapper.Map<TMapped>(x));

            return new PagedResult<TMapped>(pagedItems, items?.Count() ?? 0);
        }
    }
}
