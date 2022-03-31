using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.V1;

namespace WebApi.Controllers
{
    public class ItemController : Controller
    {
        public ItemController()
        {

        }

        [HttpGet(ApiRoutes.Items.GetAll)]
        public IActionResult GetAll()
        {
            return Ok();
        }

        [HttpGet(ApiRoutes.Items.Get)]
        public IActionResult Get([FromRoute] int itemId)
        {
           
            return Ok();
        }
    }
}
