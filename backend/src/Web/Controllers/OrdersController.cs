using Application.Enums;
using Application.Utils;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Services.DataServices.Interfaces;

namespace Web.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderService _orderService;
        private readonly IItemService _itemService;
        private readonly UserManager<ApplicationUser> _userManger;

        public OrdersController(IUnitOfWork unitOfWork, IOrderService orderService, IItemService itemService, UserManager<ApplicationUser> userManger)
        {
            _unitOfWork = unitOfWork;
            _orderService = orderService;
            _itemService = itemService;
            this._userManger = userManger;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _orderService.GetBuyerOrdersAsync());
        }

        /// <summary>
        /// GET method for creating new order
        /// </summary>
        /// <param name="id">item id</param>
        /// <returns></returns>
        public async Task<IActionResult> Create(int id)
        {
            var item = await _itemService.GetAsync(id);
            if (item == null)
            {
                TempData["error"] = "item not found";
                return RedirectToAction("Index", "Home");
            }
            var user = await _userManger.GetUserAsync(User);
            if (item.CreatedBy == user.Id)
            {
                TempData["warning"] = "item is already yours!";
                return RedirectToAction("Details", "Item", new { id = item.Id });
            }
            if (item.Sold)
            {
                TempData["warning"] = "item is already sold!";
                return RedirectToAction("Details", "Item", new { id = item.Id });
            }

            return View(new Order() { SellerId = item.CreatedBy, Item = item, PhoneNumber = user.PhoneNumber, RecieverName = user.ProfileName, ItemId = item.Id });
        }

        /// <summary>
        /// POST method for creating new order
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            var item = await _itemService.GetAsync(order.ItemId);
            if (item == null)
            {
                TempData["error"] = "item not found";
                return RedirectToAction("Index", "Home");
            }
            var user = await _userManger.GetUserAsync(User);
            if (item.CreatedBy == user.Id)
            {
                TempData["warning"] = "item is already yours!";
                return RedirectToAction("Details", "Item", new { id = item.Id });
            }
            if (item.Sold)
            {
                TempData["warning"] = "item is already sold!";
                return RedirectToAction("Details", "Item", new { id = item.Id });
            }
            order.Id = 0;
            order.OrderDate = DateUtil.GetCurrentDate();
            order.SellerId = item.CreatedBy;
            order.ShippingDate = DateUtil.GetCurrentDate();
            order.OrderStatus = OrderStatus.StatusPending;
            order.PaymentStatus = OrderStatus.PaymentStatusPending;
            await _unitOfWork.Order.AddAsync(order);
            await _unitOfWork.SaveAsync();
            TempData["success"] = "Order Creted Successfully";
            return RedirectToAction("Index");
        }

        /// <summary>
        /// GET Method for accept order
        /// </summary>
        /// <param name="id">order ID</param>
        /// <returns></returns>
        public IActionResult Accept(int id)
        {
            return View();
        }
    }
}
