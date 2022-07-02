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

        public async Task<IActionResult> Details(int id)
        {
            var order = await _unitOfWork.Order.GetWithDetails(id);
            if(order == null)
            {
                TempData["error"] = "Order not found";
                return RedirectToAction("Index");
            }
            return View(order);
        }
    }
}
