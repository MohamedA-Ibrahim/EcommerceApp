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
        private IImageEndpoint _imageEndpoint;

        public ItemController(IItemEndpoint itemEndpoint,
            ICategoryEndpoint categoryEndpoint, IImageEndpoint imageEndpoint)
        {
            _itemEndpoint = itemEndpoint;
            _categoryEndpoint = categoryEndpoint;
            _imageEndpoint = imageEndpoint;
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
                if(itemVM.Item.ImageUrl != null)
                {
                    await _imageEndpoint.DeleteImage(Path.GetFileName(itemVM.Item.ImageUrl));
                }

                itemVM.Item.ImageUrl = await _imageEndpoint.UploadImage(file);
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

            await Task.WhenAll(
                _itemEndpoint.DeleteAsync(id),
                _imageEndpoint.DeleteImage(Path.GetFileName(itemFromDb.ImageUrl)));

            return Json(new { success = true, message = "Deleted Successfully" });

        }

        #endregion
    }
}
