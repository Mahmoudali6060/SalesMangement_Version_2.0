using System;
using Database.Entities;
using Sellers;
using Microsoft.AspNetCore.Mvc;
using Shared.Classes;

namespace SalesManagementApp.Controllers
{
    public class SellersController : Controller
    {
        private ISellerOperationsRepo _sellerOperationsRepo;

        public SellersController(ISellerOperationsRepo sellerOperationsRepo)
        {
            this._sellerOperationsRepo = sellerOperationsRepo;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            var sellerList = Helper.SerializeObject(_sellerOperationsRepo.GetAll());
            return Json(sellerList);
        }

        public JsonResult GetPagedList(int currentPage, string keyword)
        {
            return Json(Helper.SerializeObject(_sellerOperationsRepo.GetAll(currentPage, keyword)));
        }

        public JsonResult GetById(long id)
        {
            var seller = Helper.SerializeObject(_sellerOperationsRepo.GetById(id));
            return Json(seller);
        }

        public JsonResult Add(Seller seller)
        {
            return Json(_sellerOperationsRepo.Add(seller));
        }

        public JsonResult Update([FromBody] Seller seller)
        {
            return Json(_sellerOperationsRepo.Update(seller));
        }

        public JsonResult Delete(long id)
        {
            return Json(_sellerOperationsRepo.Delete(id));
        }
    }
}