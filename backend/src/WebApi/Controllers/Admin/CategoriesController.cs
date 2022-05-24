using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Contracts.V1.Requests;

namespace Web.Controllers.Admin
{
    public class CategoriesController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: CategoriesController
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.Category.GetAllAsync());
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category cat)
        {
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

            var category = await _unitOfWork.Category.GetFirstOrDefaultIncludingAsync(id.Value,x=>x.AttributeTypes);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (!ModelState.IsValid)
                return BadRequest();

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
