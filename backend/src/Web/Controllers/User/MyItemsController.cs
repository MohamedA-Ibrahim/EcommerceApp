using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.ViewModels;

namespace Web.Controllers.User
{
    [Authorize]
    public class MyItemsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorageService;
        private readonly UserManager<ApplicationUser> _userManager;

        public MyItemsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _fileStorageService = fileStorageService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public async Task<ActionResult> Upsert(int? id)
        {
            ItemVM itemVM = new()
            {
                Item = new(),
                CategoryList = new SelectList(await _unitOfWork.Category.GetAllAsync(), "Id", "Name")
            };

            if (id != null && id > 0)
            {
                itemVM.Item = await _unitOfWork.Item.GetFirstOrDefaultAsync(id.Value);
            }

            return View(itemVM);
        }

        [HttpPost]
        public async Task<ActionResult> Upsert(ItemVM itemVM, IFormFile file)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (file != null)
            {
                if (itemVM.Item?.ImageUrl != null)
                {
                    await _fileStorageService.DeleteAsync(Path.GetFileName(itemVM.Item.ImageUrl));
                }
                var fileDto = new FileDto { ContentType = file.ContentType, Name = file.Name, Content = file.OpenReadStream()};
                itemVM.Item.ImageUrl = await _fileStorageService.UploadAsync(fileDto);
            }


            if (itemVM.Item.Id == 0)
            {
                await _unitOfWork.Item.AddAsync(itemVM.Item);
                TempData["success"] = "Item created succesfully";
            }
            else
            {
                _unitOfWork.Item.Update(itemVM.Item);
                TempData["success"] = "Item created succesfully";
            }

            return RedirectToAction("Index");
        }



        #region API Calls from AJAX

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _unitOfWork.Item.GetAllAsync(x => x.CreatedBy == _userManager.GetUserId(User)) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var itemFromDb = (await _unitOfWork.Item.GetAllAsync(x => x.CreatedBy == _userManager.GetUserId(User) && x.Id == id)).FirstOrDefault();
            if (itemFromDb == null)
            {
                return Json(new { success = false, message = "item not found" });
            }

            _unitOfWork.Item.Remove(await _unitOfWork.Item.GetFirstOrDefaultAsync(id));
            await _fileStorageService.DeleteAsync(Path.GetFileName(itemFromDb.ImageUrl));
            await _unitOfWork.SaveAsync();
            return Json(new { success = true, message = "Deleted Successfully" });
        }

        #endregion
    }
}
