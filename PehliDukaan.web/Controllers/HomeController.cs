//using PehliDukaan.Services;
using PehliDukaan.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PehliDukaan.Services;
using PehliDukaan.Entities;
using System.Globalization;

namespace PehliDukaan.web.Controllers {

    public class HomeController : Controller {

        CategoriesService categoryService = new CategoriesService();
        ProductsService productsService = new ProductsService();

        public ActionResult Index(int? categoryID) {

            HomeViewModel model = new HomeViewModel();

            model.FeaturedCategories = categoryService.GetFeaturedCategories();
            return View(model);
        }

        public ActionResult About() {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}