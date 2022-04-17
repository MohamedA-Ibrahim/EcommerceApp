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
                //update item
            }


            return View(itemVM);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(ItemVM itemVM, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest();
            }

            if (file != null)
            {
                itemVM.Item.ImageUrl = await _imageEndpoint.UploadImage(file);

            }

            await _itemEndpoint.CreateAsync(itemVM.Item);
            TempData["success"] = "Item created succesfully";
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

        #region API Calls

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _itemEndpoint.GetAll();
            return Json(new {data = items});
        }

        #endregion
    }
}
