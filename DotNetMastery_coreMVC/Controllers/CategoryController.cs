using DotNetMastery_Web.Data;
using DotNetMastery_Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetMastery_Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }
    }
}
