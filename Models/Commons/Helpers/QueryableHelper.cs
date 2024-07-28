using WebApi_SGI_T.Models.Commons.Request;

namespace WebApi_SGI_T.Models.Commons.Helpers
{
    public static class QueryableHelper
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, BasePaginationRequest requestPagination)
        {
            return queryable.Skip((requestPagination.NumPage - 1) * requestPagination.Records).Take(requestPagination.Records);
        }
    }
}
