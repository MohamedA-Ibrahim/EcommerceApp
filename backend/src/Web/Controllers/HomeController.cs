using Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers;
using Web.Services.DataServices.Interfaces;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemService _itemService;

        public HomeController(IUnitOfWork unitOfWork, IItemService itemService)
        {
            _unitOfWork = unitOfWork;
            _itemService = itemService;
        }

        // GET: HomeController
        public async Task<IActionResult> Index()
        {
            //await _unitOfWork.Item.GetAllIncludingAsync(x => !x.Sold, null, x => x.AttributeValues, x => x.Category, x => x.Category.AttributeTypes, x => x.ApplicationUser)
            return View((await _itemService.GetForSaleAsync()).Data);
        }

        public ActionResult Privacy()
        {
            return View();
        }

    }
}
