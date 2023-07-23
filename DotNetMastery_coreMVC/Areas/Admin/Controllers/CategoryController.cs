using DotNetMastery.DataAccess.Repository.IRepository;
using DotNetMastery.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetMastery_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            //if(obj.Name == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("name","The Display Order cannot exactly match the Name");
            //}
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Successfully Create Category";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Edit(int? CategoryId)
        {
            if (CategoryId == null || CategoryId == 0)
            {
                return NotFound();
            }

            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.CategoryId == CategoryId);
            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //Category? categoryFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Successfully Update Category";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? CategoryId)
        {
            if (CategoryId == null || CategoryId == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.CategoryId == CategoryId);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? CategoryId)
        {
            Category? obj = _unitOfWork.Category.Get(u => u.CategoryId == CategoryId);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Successfully Delete Category";
            return RedirectToAction("Index");
        }
    }
}
