using Ecommerce.WebUI.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApi.Contracts.V1.Responses;
using WebApi.Contracts.V1.Requests;

namespace Ecommerce.WebUI.ViewModels
{
    public class ItemVM
    {
        public ItemResponse ItemResponse { get;set;}
        public UpdateItemRequest UpdateItemRequest { get;set;}

        [ValidateNever]
        public SelectList CategoryList { get;set;}

    }
}
