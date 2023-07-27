using DotNetMastery.DataAccess.Repository;
using DotNetMastery.DataAccess.Repository.IRepository;
using DotNetMastery.Models;
using DotNetMastery.Models.ViewModels;
using DotNetMastery.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DotNetMastery_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();

            return View(objCompanyList);
        }

        public IActionResult Upsert(int? CompanyId)
        {
            if (CompanyId == null || CompanyId == 0)
            {
                //create
                return View(new Company());
            }
            else
            {
                //update
                Company companyObj = _unitOfWork.Company.Get(u => u.companyId == CompanyId);
                return View(companyObj);
            }
        }
        [HttpPost]
        public IActionResult Upsert(Company companyObj)
        {
            if (ModelState.IsValid)
            {
                if (companyObj.companyId == 0)
                {
                    _unitOfWork.Company.Add(companyObj);
                }
                else
                {
                    _unitOfWork.Company.Update(companyObj);
                }

                _unitOfWork.Save();
                TempData["success"] = "Company created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(companyObj);
            }
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll() 
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }
        #endregion

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = _unitOfWork.Company.Get(u => u.companyId == id);
            if (CompanyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            };

            _unitOfWork.Company.Remove(CompanyToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
    }
}



//public IActionResult Edit(int? CompanyId)
//{
//    if (CompanyId == null || CompanyId == 0)
//    {
//        return NotFound();
//    }

//    Company? CompanyFromDb = _unitOfWork.Company.Get(u => u.CompanyId == CompanyId);

//    if (CompanyFromDb == null)
//    {
//        return NotFound();
//    }
//    return View(CompanyFromDb);
//}
//[HttpPost]
//public IActionResult Edit(Company obj)
//{
//    if (ModelState.IsValid)
//    {
//        _unitOfWork.Company.Update(obj);
//        _unitOfWork.Save();
//        TempData["success"] = "Successfully Update Company";
//        return RedirectToAction("Index");
//    }
//    return View();
//}

//public IActionResult Delete(int? CompanyId)
//{
//    if (CompanyId == null || CompanyId == 0)
//    {
//        return NotFound();
//    }

//    Company? CompanyFromDb = _unitOfWork.Company.Get(u => u.CompanyId == CompanyId);

//    if (CompanyFromDb == null)
//    {
//        return NotFound();
//    }
//    return View(CompanyFromDb);
//}
//[HttpPost, ActionName("Delete")]
//public IActionResult DeletePost(int? CompanyId)
//{
//    Company? obj = _unitOfWork.Company.Get(u => u.CompanyId == CompanyId);
//    if (obj == null)
//    {
//        return NotFound();
//    }
//    _unitOfWork.Company.Remove(obj);
//    _unitOfWork.Save();
//    TempData["success"] = "Successfully Delete Company";
//    return RedirectToAction("Index");
//}