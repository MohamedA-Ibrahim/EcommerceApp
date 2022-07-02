using Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers;
using Web.Services.DataServices.Interfaces;
using Web.ViewModels;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IItemService _itemService;
        private readonly ICategoryService _categoryService;

        public HomeController(IUnitOfWork unitOfWork, IItemService itemService, ICategoryService categoryService)
        {
            _itemService = itemService;
            _categoryService = categoryService;
        }

        // GET: HomeController
        public async Task<IActionResult> Index()
        {
            var vm = new ItemsCatsVM()
            {
                Items = (await _itemService.GetForSaleAsync()).Data,
                Categories = (await _categoryService.GetAllAsync(null, null)).Data
            };
            return View(vm);
        }

        public ActionResult Privacy()
        {
            return View();
        }


    }
}
