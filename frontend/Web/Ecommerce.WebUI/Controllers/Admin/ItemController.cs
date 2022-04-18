using Ecommerce.WebUI.Api;
using Ecommerce.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Collections;
using Ecommerce.WebUI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ecommerce.WebUI.Controllers
{
    public class ItemController : Controller
    {
        private IItemEndpoint _itemEndpoint;
        private ICategoryEndpoint _categoryEndpoint;

        public ItemController(IItemEndpoint itemEndpoint,
            ICategoryEndpoint categoryEndpoint)
        {
            _itemEndpoint = itemEndpoint;
            _categoryEndpoint = categoryEndpoint;

        }

        public async Task<IActionResult> Index()
        {
             return View();  
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            ItemVM itemVM = new()
            {
                Item = new(),
                CategoryList = new SelectList(await _categoryEndpoint.GetAll(), "Id", "Name")       
            };

            if(id == null || id == 0)
            {
                return View(itemVM);
            }
            else
            {
                itemVM.Item = await _itemEndpoint.GetById(id);
            }

            return View(itemVM);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(ItemVM itemVM, IFormFile? file)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (file != null)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    itemVM.Item.Image = ms.ToArray();
                }
            }

            if(itemVM.Item.Id == 0)
                await _itemEndpoint.CreateAsync(itemVM.Item);
            else
                await _itemEndpoint.UpdateAsync(itemVM.Item);

            TempData["success"] = "Item created succesfully";
            return RedirectToAction("Index");
        }



        #region API Calls from AJAX

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _itemEndpoint.GetAll();
            return Json(new {data = items});
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var itemFromDb = await _itemEndpoint.GetById(id);
            if (itemFromDb == null)
            {
                return Json(new {success = false, message = "item not found"});
            }

                await _itemEndpoint.DeleteAsync(id);

            return Json(new { success = true, message = "Deleted Successfully" });

        }

        #endregion
    }
}
