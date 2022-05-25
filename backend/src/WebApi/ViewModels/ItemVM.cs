using Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.ViewModels
{
    public class ItemVM
    {
        public Item Item { get;set;}

        [ValidateNever]
        public SelectList CategoryList { get;set;}

    }
}
