using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: HomeController
        public ActionResult Index()
        {
            var claims = HttpContext.User.Claims;
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

    }
}
