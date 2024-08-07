using WebApi_SGI_T.Models.Commons.Request;
using System.Linq.Dynamic.Core;

namespace WebApi_SGI_T.Models.Commons.Helpers
{
    public static class OrderingHelper
    {
        public static IQueryable<TDTO> Ordering<TDTO>(BasePaginationRequest request, IQueryable<TDTO> queryable, bool pagination = false) where TDTO : class
        {
            IQueryable<TDTO> queryDto = request.Order == "desc" ? queryable.OrderBy($"{request.Sort} descending") : queryable.OrderBy($"{request.Sort} ascending");

            if (pagination) queryDto = queryDto.Paginate(request);

            return queryDto;
        }
    }
}
