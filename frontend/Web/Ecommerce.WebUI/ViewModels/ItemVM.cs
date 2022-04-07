using Ecommerce.WebUI.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.WebUI.ViewModels
{
    public class ItemVM
    {
        public Item Item {get;set;}

        public SelectList CategoryList { get;set;}

    }
}
