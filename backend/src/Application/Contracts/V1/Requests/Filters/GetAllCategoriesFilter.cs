using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.V1.Requests.Queries
{
    /// <summary>
    /// Used for filtering
    /// </summary>
    public class GetAllCategoriesFilter
    {
        //Put the field that you want to filter by
        public string UserId { get; set; }
    }
}
