using Domain.Entities;
using Web.Contracts.V1.Responses;

namespace Web.ViewModels
{
    public class CatItemsVM
    {
        public Category Category { get; set; }
        public IEnumerable<ItemResponse> Items { get; set; }
    }
}
