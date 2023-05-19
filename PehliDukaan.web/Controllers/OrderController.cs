using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PehliDukaan.Services;
using PehliDukaan.web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PehliDukaan.web.Controllers
{
    public class OrderController : Controller
    {
        private PDSignInManager _signInManager;
        private PDUserManager _userManager;

        public PDSignInManager SignInManager {
            get {
                return _signInManager ?? HttpContext.GetOwinContext().Get<PDSignInManager>();
            }
            private set {
                _signInManager = value;
            }
        }
        public PDUserManager UserManager {
            get {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<PDUserManager>();
            }
            private set {
                _userManager = value;
            }
        }

        OrderService orderService = new OrderService();
        // GET: Order
        public ActionResult Index(string userID, string status) {

            OrdersViewModel model = new OrdersViewModel();
            model.UserID = userID;
            model.Status = status;
            model.Orders = orderService.SearchOrders(userID, status);
            var totalRecords = orderService.SearchOrdersCount(userID, status);

            return View(model);
        }
        public ActionResult Details(int ID) {

            OrderDetailsViewModel model = new OrderDetailsViewModel();

            model.Order = orderService.GetOrderByID(ID);

            if (model.Order != null) {
                model.OrderBy = UserManager.FindById(model.Order.UserId);
            }

            model.AvailableStatuses = new List<string>() { "Pending", "In Progress", "Delivered" };

            return View(model);
        }

        public JsonResult ChangeStatus(string status, int ID) {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            result.Data = new { Success = orderService.UpdateOrderStatus(ID, status) };

            return result;
        }

    }
}