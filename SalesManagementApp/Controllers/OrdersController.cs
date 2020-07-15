using System;
using Database.Entities;
using Order;
using Microsoft.AspNetCore.Mvc;
using Shared.Classes;
using Order.DataServiceLayer;
using Order.Models;

namespace SalesManagementApp.Controllers
{
    public class OrdersController : Controller
    {
        private IOrderOperationsDSL _orderOperationsDSL;

        public OrdersController(IOrderOperationsDSL orderOperationsRepo)
        {
            this._orderOperationsDSL = orderOperationsRepo;
        }

        public ActionResult Index(int? today)
        {
            ViewBag.today = today;
            return View();
        }

        public IActionResult Details(int? id)
        {
            ViewBag.Id = id;
            return View();
        }

        public JsonResult List()
        {
            var orderList = Helper.SerializeObject(_orderOperationsDSL.GetAll());
            return Json(orderList);
        }
        public JsonResult GetPagedList(int currentPage, string keyword,bool isToday)
        {
            return Json(Helper.SerializeObject(_orderOperationsDSL.GetAll(currentPage, keyword, isToday)));
        }
        //public JsonResult GetAllDaily()
        //{
        //    var orderList = Helper.SerializeObject(_orderOperationsDSL.GetAllDaily());
        //    return Json(orderList);
        //}
        public JsonResult GetById(long id)
        {
            var order = Helper.SerializeObject(_orderOperationsDSL.GetById(id));
            return Json(order);
        }

        public JsonResult Add(OrderDTO order)
        {
            return Json(_orderOperationsDSL.Add(order));
        }

        public JsonResult Update(OrderDTO order)
        {
            return Json(_orderOperationsDSL.Update(order));
        }

        public JsonResult Delete(long id)
        {
            return Json(_orderOperationsDSL.Delete(id));
        }
    }
}