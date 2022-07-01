using Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class OrderReportController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderReportController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.Order.GetAllIncludingAsync(null, null, x => x.Buyer, x => x.Item, x => x.Item.Seller));
        }
    }
}
