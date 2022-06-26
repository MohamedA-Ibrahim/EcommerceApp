using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Services;
using Web.Services.DataServices.Interfaces;
using Web.ViewModels;

namespace Web.Controllers.User
{
    [Authorize]
    public class ItemsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorageService;
        private readonly IAttributeTypeServices _attributeTypeServices;
        private readonly IItemService _itemServices;
        private readonly UserManager<ApplicationUser> _userManager;
        public ItemsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IFileStorageService fileStorageService, IAttributeTypeServices attributeTypeServices, IItemService itemServices)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _fileStorageService = fileStorageService;
            _attributeTypeServices = attributeTypeServices;
            _itemServices = itemServices;
        }
        public async Task<IActionResult> Index()
        {
            var userID = _userManager.GetUserId(User);
            return View(await _unitOfWork.Item.GetAllIncludingAsync(filter: x => x.SellerId == userID, paginationFilter: null, x => x.Category));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var item = await _itemServices.GetWithDetailsAsync(id);
            if (item == null)
            {
                TempData["warning"] = "item not found!";
                return RedirectToAction("Index", "Home");
            }
            return View(item);
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
                var userID = _userManager.GetUserId(User);
                itemVM.Item = (await _unitOfWork.Item.GetAllIncludingAsync(x => x.Id == id && x.SellerId == userID, null, x => x.AttributeValues)).FirstOrDefault();
            }

            return View(itemVM);
        }

        [HttpPost]
        public async Task<ActionResult> Upsert(ItemVM itemVM, IFormFile file)
        {
            if (!ModelState.IsValid)
                return View(itemVM);

            if (file != null && file.Length > 0)
            {
                if (itemVM.Item?.ImageUrl != null)
                {
                    await _fileStorageService.DeleteAsync(Path.GetFileName(itemVM.Item.ImageUrl));
                }
                var fileDto = new FileDto { ContentType = file.ContentType, Name = file.Name, Content = file.OpenReadStream() };
                itemVM.Item.ImageUrl = await _fileStorageService.UploadAsync(fileDto);
            }


            if (itemVM.Item.Id == 0)
            {
                await _unitOfWork.Item.AddAsync(itemVM.Item);
                TempData["success"] = "Item created succesfully";
            }
            else
            {
                int oldCategoryID = (await _unitOfWork.Item.GetFirstOrDefaultAsync(itemVM.Item.Id)).CategoryId;
                if (itemVM.Item.AttributeValues != null && itemVM.Item.CategoryId == oldCategoryID)
                {
                    foreach (var attValue in await _unitOfWork.AttributeValue.GetAllAsync(x => x.ItemId == itemVM.Item.Id))
                    {
                        attValue.Value = itemVM.Item.AttributeValues.Where(x=>x.AttributeTypeId == attValue.AttributeTypeId).FirstOrDefault().Value;
                    }
                }
                else if(itemVM.Item.CategoryId != oldCategoryID)
                {
                    //remove old attribute value
                    _unitOfWork.AttributeValue.RemoveRange(await _unitOfWork.AttributeValue.GetAllAsync(x => x.ItemId == itemVM.Item.Id));
                    await _unitOfWork.SaveAsync();

                    //assign itemID for all attribute values
                    foreach(var attValue in itemVM.Item.AttributeValues)
                    {
                        attValue.ItemId = itemVM.Item.Id;
                    }

                    //add attribute values to database
                    await _unitOfWork.AttributeValue.AddRangeAsync(itemVM.Item.AttributeValues.ToList());
                    await _unitOfWork.SaveAsync();
                }
                _unitOfWork.Item.Update(itemVM.Item);
                await _unitOfWork.SaveAsync();
                TempData["success"] = "Item updated succesfully";
            }

            await _unitOfWork.SaveAsync();

            return RedirectToAction("Index");
        }



        #region API Calls from AJAX

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _unitOfWork.Item.GetAllAsync(x => x.SellerId == _userManager.GetUserId(User)) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var itemFromDb = (await _unitOfWork.Item.GetAllAsync(x => x.SellerId == _userManager.GetUserId(User) && x.Id == id)).FirstOrDefault();
            if (itemFromDb == null)
            {
                return Json(new { success = false, message = "item not found" });
            }

            _unitOfWork.Item.Remove(await _unitOfWork.Item.GetFirstOrDefaultAsync(id));
            await _fileStorageService.DeleteAsync(Path.GetFileName(itemFromDb.ImageUrl));
            await _unitOfWork.SaveAsync();
            return Json(new { success = true, message = "Deleted Successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetGategoryAttributes(int id)
        {
            var attributes = await _attributeTypeServices.GetByCategoryAsync(id);
            return PartialView("_GetegoryAttributes", attributes);
        }

        #endregion
    }
}
