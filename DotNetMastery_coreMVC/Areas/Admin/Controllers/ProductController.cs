using DotNetMastery.DataAccess.Repository.IRepository;
using DotNetMastery.Models;
using DotNetMastery.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DotNetMastery_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();

            return View(objProductList);
        }

        public IActionResult Upsert(int? ProductId)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.CategoryId.ToString(),
                }),
                Product = new Product()
            };
            if (ProductId == null || ProductId == 0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitOfWork.Product.Get(u => u.ProductId == ProductId);
                return View(productVM);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM ProductVM, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(ProductVM.Product.ImageUrl))
                    {
                        var oldImgPath = Path.Combine(wwwRootPath, ProductVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImgPath))
                        {
                            System.IO.File.Delete(oldImgPath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    ProductVM.Product.ImageUrl = @"\images\product\" + fileName;
                }

                if (ProductVM.Product.ProductId == 0)
                {
                    _unitOfWork.Product.Add(ProductVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(ProductVM.Product);
                }

                _unitOfWork.Save();
                TempData["success"] = "Successfully Create Product";
                return RedirectToAction("Index");
            }
            else
            {
                ProductVM.CategoryList = _unitOfWork.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.CategoryId.ToString(),
                });
                return View(productVM);
            }
            
        }

        public IActionResult Delete(int? ProductId)
        {
            if (ProductId == null || ProductId == 0)
            {
                return NotFound();
            }

            Product? productFromDb = _unitOfWork.Product.Get(u => u.ProductId == ProductId);

            if(productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }
        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePost(int? ProductId)
        {
            Product? obj = _unitOfWork.Product.Get(u => u.ProductId == ProductId);
            if(obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Successfully Delete Product";
            return RedirectToAction("Index");
        }
    }
}



//public IActionResult Edit(int? ProductId)
//{
//    if (ProductId == null || ProductId == 0)
//    {
//        return NotFound();
//    }

//    Product? productFromDb = _unitOfWork.Product.Get(u => u.ProductId == ProductId);

//    if (productFromDb == null)
//    {
//        return NotFound();
//    }
//    return View(productFromDb);
//}
//[HttpPost]
//public IActionResult Edit(Product obj)
//{
//    if (ModelState.IsValid)
//    {
//        _unitOfWork.Product.Update(obj);
//        _unitOfWork.Save();
//        TempData["success"] = "Successfully Update Product";
//        return RedirectToAction("Index");
//    }
//    return View();
//}