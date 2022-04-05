using Ecommerce.WebUI.Api;
using Ecommerce.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Collections;

namespace Ecommerce.WebUI.Controllers
{
    [Area("Admin")]
    public class ItemController : Controller
    {
        private IItemEndpoint _itemEndpoint;
        private ICategoryEndpoint _categoryEndpoint;

        public ItemController(IItemEndpoint itemEndpoint, ICategoryEndpoint categoryEndpoint)
        {
            _itemEndpoint = itemEndpoint;
            _categoryEndpoint = categoryEndpoint;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _itemEndpoint.GetAll();
             return View(items);  
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Item item = new ();
            var CategoryList = await _categoryEndpoint.GetAll();

            SelectList categoryList = new SelectList(CategoryList, "Id", "Name");


            if(id == null || id == 0)
            {
                //create item
                ViewBag.CategoryList = categoryList;
                return View(item);
            }
            else
            {
                //update item
            }


            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Item item)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            await _itemEndpoint.UpdateAsync(item);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var itemFromDb = await _itemEndpoint.GetById(id);
            if (itemFromDb == null)
            {
                return NotFound();
            }

            return View(itemFromDb);

        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(int id)
        {
            await _itemEndpoint.DeleteAsync(id);

            return RedirectToAction("Index");

        }
    }
}
