using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.V1.Requests.Queries
{
    /// <summary>
    /// A query for filtering items
    /// </summary>
    public class GetAllItemsFilter
    {
        public string Name { get; set; }
    }
}
