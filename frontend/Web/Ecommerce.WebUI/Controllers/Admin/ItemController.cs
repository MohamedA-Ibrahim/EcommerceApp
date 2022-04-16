using Ecommerce.WebUI.Api;
using Ecommerce.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Collections;
using Ecommerce.WebUI.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.WebUI.Controllers
{
    public class ItemController : Controller
    {
        private IItemEndpoint _itemEndpoint;
        private ICategoryEndpoint _categoryEndpoint;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ItemController(IItemEndpoint itemEndpoint, ICategoryEndpoint categoryEndpoint, IWebHostEnvironment hostEnvironment)
        {
            _itemEndpoint = itemEndpoint;
            _categoryEndpoint = categoryEndpoint;
            _hostEnvironment = hostEnvironment;
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
        public async Task<IActionResult> Upsert(ItemVM itemVM, IFormFile? file)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            //Get image
            string wwRootPath = _hostEnvironment.WebRootPath;
            if(file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwRootPath, @"images\items");
                var extension = Path.GetExtension(file.FileName);

                using(var fileStreams = new FileStream(Path.Combine(uploads, fileName+extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }

                itemVM.Item.ImageUrl = @"\images\items\" + fileName + extension;
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
    }
}
