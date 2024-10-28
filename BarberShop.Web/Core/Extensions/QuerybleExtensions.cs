using BarberShop.Web.Core.Pagination;

namespace BarberShop.Web.Core.Extensions
{
    public static class QuerybleExtensions
    {

        public static IQueryable <T> Paginate <T> (this IQueryable<T> query, PaginationRequest request)
        {

            return query.Skip((request.Page - 1) * request.RecordsPerPage)
                        .Take(request.RecordsPerPage);
        }
    }
}
