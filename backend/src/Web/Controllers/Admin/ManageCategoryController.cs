﻿using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Contracts.V1.Requests;
using Web.Services.DataServices.Interfaces;

namespace Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageCategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorageService;
        private readonly ICategoryService _categoryService;

        public ManageCategoryController(IUnitOfWork unitOfWork, IFileStorageService fileStorageService, ICategoryService categoryService)
        {
            _unitOfWork = unitOfWork;
            _fileStorageService = fileStorageService;
            _categoryService = categoryService;
        }

        // GET: CategoriesController
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.Category.GetAllIncludingAsync(null, null, x => x.AttributeTypes));
        }


        public IActionResult Create()
        {
            return View(new Category() { AttributeTypes = new List<AttributeType>() { new AttributeType() } });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category cat, IFormFile file)
        {
            if (file != null)
            {
                var fileDto = new FileDto()
                {
                    ContentType = file.ContentType,
                    Name = file.Name,
                    Content = file.OpenReadStream()
                };
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

            if (file != null && file.Length > 0)
            {
                if (string.IsNullOrWhiteSpace(category?.ImageUrl))
                {
                    await _fileStorageService.DeleteAsync(Path.GetFileName(category.ImageUrl));
                }

                var fileDto = new FileDto()
                {
                    ContentType = file.ContentType,
                    Name = file.Name,
                    Content = file.OpenReadStream()
                };
                category.ImageUrl = await _fileStorageService.UploadAsync(fileDto);
            }
            else
                category.ImageUrl = "";

            var oldAttributes = await _unitOfWork.AttributeType.GetAllAsync(x => x.CategoryId == category.Id);

            var newAttributes = new List<AttributeType>();
            newAttributes.AddRange(category.AttributeTypes);
            newAttributes.ForEach(x => { x.CategoryId = category.Id; });
            for (int i = 0; i < oldAttributes.Count || i < newAttributes.Count; i++)
            {
                if (i < oldAttributes.Count && i < newAttributes.Count)
                {
                    oldAttributes[i].Name = newAttributes[i].Name;
                }
                else if(i < oldAttributes.Count)
                {
                    _unitOfWork.AttributeType.RemoveRange(oldAttributes.Skip(i));
                    break;
                }
                else if(i < newAttributes.Count)
                {
                    await _unitOfWork.AttributeType.AddRangeAsync(newAttributes.Skip(i).ToList());
                    break;
                }
            }
            await _unitOfWork.SaveAsync();

            await _categoryService.UpdateAsync(category.Id, new UpdateCategoryRequest() { Description = category.Description, Name = category.Name, ImageUrl = category.ImageUrl }, true);

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
            await _unitOfWork.SaveAsync();
            return RedirectToAction("Index");

        }
    }
}
