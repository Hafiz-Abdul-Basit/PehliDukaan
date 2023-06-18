using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using PehliDukaan.Entities;
using PehliDukaan.Services;
using PehliDukaan.web.Migrations;
using PehliDukaan.web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace PehliDukaan.web.Controllers
{
    public class DashboardController : Controller {

        DashboardService service = new DashboardService();

        private PDUserManager _userManager;

        private PDRoleManager _roleManager;


        public DashboardController() {
        }

        public DashboardController(PDUserManager userManager, PDRoleManager roleManager) {
            UserManager = userManager;
            RoleManager = roleManager;
        }


        public PDUserManager UserManager {
            get {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<PDUserManager>();
            }
            private set {
                _userManager = value;
            }
        }


        public PDRoleManager RoleManager {
            get {
                return _roleManager ?? HttpContext.GetOwinContext().Get<PDRoleManager>();
            }
            private set {
                _roleManager = value;
            }
        }
        // GET: Dashboard

        [Authorize(Roles = "Administrator")]
        public ActionResult Index() {
            DashboardViewModel model = new DashboardViewModel();

            model.CategoriesCount = service.GetCategoriesCount();
            model.ProductsCount = service.GetProductsCount();

            model.OrdersCount = service.GetOrdersCount();
            model.TotalEarnings = service.GetTotalEarnings();



            return View(model);


        }

        [Authorize(Roles = "Administrator")]

        public ActionResult Users(string roleID, string searchTerm) {


            UsersViewModel model = new UsersViewModel();

            model.RoleID = roleID;
            model.SearchTerm = searchTerm;
            model.Roles = RoleManager.Roles.ToList();

            return View(model);
        }

        [Authorize(Roles = "Administrator")]

        public ActionResult UsersListing(string roleID, string searchTerm) {

            UsersListingViewModel model = new UsersListingViewModel();

            //model.Users = UserManager.Users.ToList();
            model.RoleID = roleID;
            model.SearchTerm = searchTerm;

            var users = UserManager.Users;

            if (!string.IsNullOrEmpty(roleID)) {

                users = users.Where(x => x.Roles.FirstOrDefault(y => y.RoleId == roleID) != null);

            }



            if (!string.IsNullOrEmpty(searchTerm)) {

                users = users.Where(x => x.Email.ToLower().Contains(searchTerm.ToLower()));

            }
            model.Users = users.ToList();

            return PartialView(model);
        }



        [Authorize(Roles = "Administrator")]

        public ActionResult Reports() {
            ReportService reportService = new ReportService();

            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            var orders = reportService.GetOrdersByMonthAndProduct(currentMonth, currentYear);
            var productReport = reportService.GenerateProductReport(orders);

            var model = new ReportsViewModel {
                CurrentMonth = currentMonth,
                CurrentYear = currentYear,
                ProductReport = productReport
            };

            return View(model);
        }

 

        [Authorize(Roles = "Administrator")]

        public ActionResult Roles(string searchTerm) {


            RolesListingViewModel model = new RolesListingViewModel();
            model.SearchTerm = searchTerm;
            return View(model);
        }

        [Authorize(Roles = "Administrator")]

        public ActionResult RolesListing(string searchTerm) {

            RolesListingViewModel model = new RolesListingViewModel();

            model.SearchTerm = searchTerm;

            var roles = RoleManager.Roles;

            if (!string.IsNullOrEmpty(searchTerm)) {

                roles = roles.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));

            }
            model.Roles = roles.ToList();

            return PartialView(model);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> UserDetails(string userID, bool isPartial = false) {


            UserDetailsViewModel model = new UserDetailsViewModel();

            var user = await UserManager.FindByIdAsync(userID);

            if (user != null) {

                model.User = user;
            }

            if (isPartial || Request.IsAjaxRequest()) {

                return PartialView("_UserDetails", model);
            }

            else {

                return View(model);

            }

        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> UserRoles(string userID) {


            UserRolesViewModel model = new UserRolesViewModel();

            model.AvailableRoles = RoleManager.Roles.ToList();

            if (!string.IsNullOrEmpty(userID)) {

                model.User = await UserManager.FindByIdAsync(userID);

                if (model.User != null) {

                    model.UserRoles = model.User.Roles.Select(userRole => model.AvailableRoles.FirstOrDefault(role => role.Id == userRole.RoleId)).ToList();

                }

            }
            return PartialView("_UserRoles",model);

        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> AssignUserRole(string userID, string roleID) {

            if (!string.IsNullOrEmpty(userID) && !string.IsNullOrEmpty(roleID)) {

                var user = await UserManager.FindByIdAsync(userID);

                if (user != null) {

                    var role = await RoleManager.FindByIdAsync(roleID);

                    if (role != null) {

                    await UserManager.AddToRoleAsync(userID, role.Name);

                    }

                }
            }

            return RedirectToAction("UserRoles", new { userID = userID });

        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> DeleteUserRole(string userID, string roleID) {

            if (!string.IsNullOrEmpty(userID) && !string.IsNullOrEmpty(roleID)) {
                var user = await UserManager.FindByIdAsync(userID);

                if (user != null) {

                    var role = await RoleManager.FindByIdAsync(roleID);

                    if (role != null) {

                        await UserManager.RemoveFromRoleAsync(userID, role.Name);

                    }

                }
            }

            return RedirectToAction("UserRoles", new { userID = userID });

        }


    }
}