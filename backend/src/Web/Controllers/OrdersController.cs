using Application.Common.Interfaces;
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
        #region Constractor and properities
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderService _orderService;
        private readonly IItemService _itemService;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManger;

        public OrdersController(IUnitOfWork unitOfWork, IOrderService orderService, IItemService itemService, UserManager<ApplicationUser> userManger, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _orderService = orderService;
            _itemService = itemService;
            this._userManger = userManger;
            _currentUserService = currentUserService;
        }
        #endregion

        #region My Orders
        public async Task<IActionResult> Index()
        {
            return View(await _orderService.GetBuyerOrdersAsync());
        }
        #endregion

        #region Create Order
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
            if (item.SellerId == user.Id)
            {
                TempData["warning"] = "item is already yours!";
                return RedirectToAction("Details", "Item", new { id = item.Id });
            }
            if (item.Sold)
            {
                TempData["warning"] = "item is already sold!";
                return RedirectToAction("Details", "Item", new { id = item.Id });
            }

            return View(new Order() { Item = item, PhoneNumber = user.PhoneNumber, RecieverName = user.ProfileName, ItemId = item.Id });
        }

        /// <summary>
        /// POST method for creating new order
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            var item = await _itemService.GetAsync(order.ItemId);
            if (item == null)
            {
                TempData["error"] = "item not found";
                return RedirectToAction("Index", "Home");
            }
            var user = await _userManger.GetUserAsync(User);
            if (item.SellerId == user.Id)
            {
                TempData["warning"] = "item is already yours!";
                return RedirectToAction("Details", "Item", new { id = item.Id });
            }
            if (item.Sold)
            {
                TempData["warning"] = "item is already sold!";
                return RedirectToAction("Details", "Item", new { id = item.Id });
            }
            order.BuyerId = user.Id;
            order.Id = 0;
            order.OrderDate = DateUtil.GetCurrentDate();
            order.OrderStatus = OrderStatus.Pending;
            order.PaymentStatus = PaymentStatus.Pending;

            if (ModelState.IsValid)
            {
                await _unitOfWork.Order.AddAsync(order);
                await _unitOfWork.SaveAsync();
                TempData["success"] = "Order Creted Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                order.Item = item;
                return View(order);
            }
        }
        #endregion

        #region View Details

        public async Task<IActionResult> Details(int id)
        {
            var order = await _unitOfWork.Order.GetWithDetails(id);
            if (order == null)
            {
                TempData["error"] = "Order not found!";
                return RedirectToAction("Index");
            }
            if (order.Item.SellerId != _currentUserService.UserId && order.BuyerId != _currentUserService.UserId)
            {
                TempData["warning"] = "you are not the seller or buyer in this order";
                return RedirectToAction("Index");
            }
            return View(order.Item.SellerId == _currentUserService.UserId ? "DetailsSeller" : "DetailsBuyer", order);
        }

        #endregion

        #region Seller Operations

        #region Start processing Order
        [HttpPost, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> StartProcessing(int id)
        {
            var result = await _orderService.StartProcessingAsync(id);
            if (result.success)
            {
                TempData["success"] = result.message;
            }
            else
            {
                TempData["warning"] = result.message;
            }
            return RedirectToAction("Details", new { id = id });
        }
        #endregion

        #region Confirm Payment Order
        [HttpPost, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ConfirmPayment(int id)
        {
            var result = await _orderService.ConfirmPaymentAsync(id);
            if (result.success)
            {
                TempData["success"] = result.message;
            }
            else
            {
                TempData["warning"] = result.message;
            }
            return RedirectToAction("Details", new { id = id });
        }
        #endregion

        #region Ship Order
        [HttpPost, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Ship(int id)
        {
            var result = await _orderService.ShipOrderAsync(id);
            if (result.success)
            {
                TempData["success"] = result.message;
            }
            else
            {
                TempData["warning"] = result.message;
            }
            return RedirectToAction("Details", new { id = id });
        }
        #endregion

        #region Reject Order 
        [HttpPost, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var result = await _orderService.RejectOrderAsync(id);
            if (result.success)
            {
                TempData["success"] = result.message;
            }
            else
            {
                TempData["warning"] = result.message;
            }
            return RedirectToAction("Details", new { id = id });
        }
        #endregion

        #endregion

        #region Buyer Operations
        #region Cancel Order 
        /// <summary>
        /// POST Method for cancel the order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _orderService.CancelOrderAsync(id);
            if (result.success)
            {
                TempData["success"] = result.message;
            }
            else
            {
                TempData["warning"] = result.message;
            }
            return RedirectToAction("Details",new { id = id });
        }
        #endregion
        #endregion

    }
}
