//using PehliDukaan.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PehliDukaan.Services;
using PehliDukaan.Entities;
using PehliDukaan.web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PehliDukaan.Services.Models;

namespace PehliDukaan.web.Controllers
{
    public class ShopController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager {
            get {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set {
                _signInManager = value;
            }
        }
        public ApplicationUserManager UserManager {
            get {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set {
                _userManager = value;
            }
        }

        ProductsService productsService = new ProductsService();
        CategoriesService categoriesService = new CategoriesService();
        ShopService shopService = new ShopService();
         

        public ActionResult Index(string searchTerm, int? minimumPrice, int? maximumPrice, int? categoryID, int? sortBy) {
        
            ShopViewModel model = new ShopViewModel();

            model.SearchTerm = searchTerm;
            model.FeaturedCategories = categoriesService.GetFeaturedCategories();
            model.MaximumPrice = productsService.GetMaximumPrice();

           
            model.SortBy = sortBy;
            model.CategoryID = categoryID;

            model.Products = productsService.SearchProducts(searchTerm, minimumPrice, maximumPrice, categoryID, sortBy);

            return View(model);
        }

        //[Authorize]
        public ActionResult AddToCart() {
            CheckoutViewModel model = new CheckoutViewModel();

            var CartProductsCookie = HttpUtility.UrlDecode(Request.Cookies["CartProducts"].Value);
            IEnumerable<ProductCartCookie> cartItems = JsonConvert.DeserializeObject<IEnumerable<ProductCartCookie>>(CartProductsCookie);

            if (cartItems == null || cartItems.Any() == false) {
                return View(model);
            }

            model.CartProducts = productsService.GetCartProducts(cartItems);
            model.User = UserManager.FindById(User.Identity.GetUserId());

            return View(model);
        }

        // GET: Shop
        [Authorize]
        public ActionResult Checkout() {
            CheckoutViewModel model = new CheckoutViewModel();

            var CartProductsCookie = Request.Cookies["CartProducts"];
            IEnumerable<ProductCartCookie> cartItems = JsonConvert.DeserializeObject<IEnumerable<ProductCartCookie>>(CartProductsCookie.Value);

            if (cartItems == null || cartItems.Any() == false) {
                return View(model);
            }

            model.CartProducts = productsService.GetCartProducts(cartItems);
            model.User = UserManager.FindById(User.Identity.GetUserId());

            return View(model);
        }

        public JsonResult PlaceOrder(string productIDs) {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            if (!string.IsNullOrEmpty(productIDs)) {
                var productQuantities = productIDs.Split('-').Select(x => int.Parse(x)).ToList();

                var boughtProducts = productsService.GetProducts(productQuantities.Distinct().ToList());

                Order newOrder = new Order();
                newOrder.UserId = User.Identity.GetUserId();
                newOrder.OrderedAt = DateTime.Now;
                newOrder.Status = "Pending";
                newOrder.TotalAmount = boughtProducts.Sum(x => x.Price * productQuantities.Where(productID => productID == x.Id).Count());

                newOrder.Items = new List<OrderItem>();
                newOrder.Items.AddRange(boughtProducts.Select(x => new OrderItem() { ProductId = x.Id, Quantity = productQuantities.Where(productID => productID == x.Id).Count() }));

                var rowsEffected = shopService.SaveOrder(newOrder);

                result.Data = new { Success = true, Rows = rowsEffected };
            }
            else {
                result.Data = new { Success = false };
            }

            return result;
        }

    }
}