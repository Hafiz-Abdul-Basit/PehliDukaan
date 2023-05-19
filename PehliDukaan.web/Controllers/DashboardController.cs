using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using PehliDukaan.Entities;
using PehliDukaan.Services;
using PehliDukaan.web.Migrations;
using PehliDukaan.web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PehliDukaan.web.Controllers
{
    public class DashboardController : Controller
    {

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
        public ActionResult Index()
        {
            DashboardViewModel model = new DashboardViewModel();

            model.CategoriesCount = service.GetCategoriesCount();
            model.ProductsCount = service.GetProductsCount();

            model.OrdersCount = service.GetOrdersCount();


            return View(model);
        }


        public ActionResult Users(string roleID, string searchTerm) {


            UsersViewModel model = new UsersViewModel();

            model.RoleID = roleID;
            model.SearchTerm = searchTerm;
            model.Roles = RoleManager.Roles.ToList();

            return View(model);
        }

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

        public ActionResult Roles( string searchTerm) {


            RolesListingViewModel model = new RolesListingViewModel();
            model.SearchTerm = searchTerm;
            return View(model);
        }

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

        public async Task<ActionResult> UserDetails(string userID) {


            UserDetailsViewModel model = new UserDetailsViewModel();
            
            var user = await UserManager.FindByIdAsync(userID);

            if (user != null) {

                model.User = user;
            }

            return View(model);
        }
    }
}