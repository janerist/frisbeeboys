using System.Collections.Generic;

namespace Frisbeeboys.Api.Api.Shared
{
    public record PagedResponse<T>(IList<T> Data, int Page, int PageSize, int TotalCount);
}