using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using System.Drawing.Printing;

namespace OpenPrintServerVueNet.Server.Classes
{
    public class PaginationResults<T> where T : class
    {
        public IEnumerable<T>? Results { get; set; }
        public PageInfo? PageInfo { get; set; }

        public PaginationResults(IEnumerable<T> data, int page = 1, int pageSize = 20)
        {
            Results = data.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = data.Count() };
        }
    }
}
