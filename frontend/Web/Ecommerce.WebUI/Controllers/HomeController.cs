using Ecommerce.WebUI.Api;
using Ecommerce.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Ecommerce.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IItemEndpoint _itemEndpoint;
        private ICategoryEndpoint _categoryEndpoint;
        public HomeController(ILogger<HomeController> logger, IItemEndpoint itemEndpoint, ICategoryEndpoint categoryEndpoint)
        {
            _logger = logger;
            _itemEndpoint = itemEndpoint;
            _categoryEndpoint = categoryEndpoint;
        }

        public async Task<IActionResult> Index()
        {
            var itemList = await _itemEndpoint.GetAll();
            return View(itemList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}