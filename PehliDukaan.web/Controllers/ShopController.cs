//using PehliDukaan.Services;
using PehliDukaan.Database.DataService;
using PehliDukaan.web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace PehliDukaan.web.Controllers
{

  
    public class ShopController : Controller
    {
        //ProductsService productsService = new ProductsService();    

        public ActionResult Index(string searchTerm, int? minimumPrice, int? maximumPrice, int? categoryID, int? sortBy) {
        
            ShopViewModel model = new ShopViewModel();
            ProductsService productsService = new ProductsService();
            CategoriesService categoriesService = new CategoriesService();

            model.SearchTerm = searchTerm;
            model.FeaturedCategories = categoriesService.GetFeaturedCategories();
            model.MaximumPrice = productsService.GetMaximumPrice();

           
            model.SortBy = sortBy;
            model.CategoryID = categoryID;

            model.Products = productsService.SearchProducts(searchTerm, minimumPrice, maximumPrice, categoryID, sortBy);

            return View(model);
        }



        // GET: Shop
        public ActionResult Checkout()
        {
            CheckoutViewModel model = new CheckoutViewModel();

            var CartProductsCookie = Request.Cookies["CartProducts"];

            if (CartProductsCookie != null) {

                // var productIDs = CartProductsCookie.Value;
                //var ids = productIDs.Split('-');
                //List<int> pIDs = ids.Select(x => int.Parse(x)).ToList();

                model.CartProductIDs = CartProductsCookie.Value.Split('-').Select(x => int.Parse(x)).ToList();

            }

            return View(model);
        }
    }
}