using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Contracts.V1.Requests;

namespace Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageCategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorageService;

        public ManageCategoryController(IUnitOfWork unitOfWork, IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _fileStorageService = fileStorageService;
        }

        // GET: CategoriesController
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.Category.GetAllIncludingAsync(null,null,x=>x.AttributeTypes));
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category cat,IFormFile file)
        {
            if (file != null)
            {
                var fileDto = new FileDto()
                {
                    ContentType = file.ContentType,
                    Name = file.Name,
                    Content = Stream.Null
                };
                await file.CopyToAsync(fileDto.Content);
                cat.ImageUrl = await _fileStorageService.UploadAsync(fileDto);
            }
            else
                cat.ImageUrl = "";

            if (ModelState.IsValid)
            {
                await _unitOfWork.Category.AddAsync(cat);
                await _unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }
            return View(cat);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var category = await _unitOfWork.Category.GetFirstOrDefaultIncludingAsync(id.Value, x => x.AttributeTypes);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category, IFormFile file)
        {
            if (!ModelState.IsValid)
                return View(category);

            if (file != null)
            {
                if (string.IsNullOrWhiteSpace(category?.ImageUrl))
                {
                    await _fileStorageService.DeleteAsync(Path.GetFileName(category.ImageUrl));
                }

                var fileDto = new FileDto()
                {
                    ContentType = file.ContentType,
                    Name = file.Name,
                    Content = Stream.Null
                };
                await file.CopyToAsync(fileDto.Content);
                category.ImageUrl = await _fileStorageService.UploadAsync(fileDto);
            }
            else
                category.ImageUrl = "";

            _unitOfWork.Category.Update(category);
            await _unitOfWork.SaveAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var categoryFromDb = await _unitOfWork.Category.GetFirstOrDefaultAsync(id.Value);
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);

        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(int id)
        {
            _unitOfWork.Category.Remove(await _unitOfWork.Category.GetFirstOrDefaultAsync(id));
            return RedirectToAction("Index");

        }
    }
}
