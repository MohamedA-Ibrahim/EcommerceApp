using Domain.Entities;
using Ecommerce.WebUI.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.WebUI.ViewModels
{
    public class ItemVM
    {
        public Item Item {get;set;}

        [ValidateNever]
        public SelectList CategoryList { get;set;}

    }
}
