using System;
using Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Shared.Classes;
using Purechase;

namespace SalesManagementApp.Controllers
{
    public class PurechasesController : Controller
    {
        private IPurechasesOperationsRepo _purechasesOperationsRepo;

        public PurechasesController(IPurechasesOperationsRepo purechasesHeaderOperationsRepo)
        {
            this._purechasesOperationsRepo = purechasesHeaderOperationsRepo;
        }

        public ActionResult Index(int? today)
        {
            ViewBag.today = today;
            return View();
        }

        public JsonResult List()
        {
            var purechasesHeaderList = Helper.SerializeObject(_purechasesOperationsRepo.GetAll());
            return Json(purechasesHeaderList);
        }

        public JsonResult GetPagedList(int currentPage, string keyword, bool isToday)
        {
            return Json(Helper.SerializeObject(_purechasesOperationsRepo.GetAll(currentPage, keyword, isToday)));
        }

        public JsonResult GetById(long id)
        {
            var purechasesHeader = Helper.SerializeObject(_purechasesOperationsRepo.GetById(id));
            return Json(purechasesHeader);
        }

        public JsonResult Update(PurechasesHeader purechasesHeader)
        {
            return Json(_purechasesOperationsRepo.Update(purechasesHeader));
        }

        public JsonResult UpdateInPrinting(PurechasesHeader purechasesHeader)
        {
            return Json(_purechasesOperationsRepo.UpdateInPrinting(purechasesHeader));
        }

        //public JsonResult Delete(long id)
        //{
        //    return Json(_purechasesOperationsRepo.Delete(id));
        //}

    }
}