using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class PaginationFilter
    {
        public PaginationFilter()
        {
            PageNumber = 1;
            PageSize = 10;
        }

        public PaginationFilter(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize > 100 ? 100 : pageSize;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
