using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUriService
    {
        Uri GetCategoryUri(string categoryId);
        Uri GetAllCategoriesUri(PaginationQuery pagination = null);
    }
}
