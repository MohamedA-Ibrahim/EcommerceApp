using Web.Contracts.V1.Responses;

namespace Web.ViewModels
{
    public class ItemsCatsVM
    {
        public IEnumerable<ItemResponse> Items { get; set; }
        public IEnumerable<CategoryResponse> Categories { get; set; }
    }
}
