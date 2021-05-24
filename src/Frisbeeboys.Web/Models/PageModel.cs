using System.Collections.Generic;

namespace Frisbeeboys.Web.Models
{
    public record PageModel<T>(IList<T> Data, int Page, int PageSize, int TotalCount);
}