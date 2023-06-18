//using PehliDukaan.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using PehliDukaan.Entities;
using PehliDukaan.Services;
using PehliDukaan.Services.Models;
using PehliDukaan.Services.Models.Requests;
using PehliDukaan.web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PehliDukaan.web.Controllers {
    public class ShopController : Controller {
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


        [Authorize]
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

            var CartProductsCookie = HttpUtility.UrlDecode(Request.Cookies["CartProducts"].Value);
            IEnumerable<ProductCartCookie> cartItems = JsonConvert.DeserializeObject<IEnumerable<ProductCartCookie>>(CartProductsCookie);

            if (cartItems == null || cartItems.Any() == false) {
                return View(model);
            }

            model.CartProducts = productsService.GetCartProducts(cartItems);
            model.User = UserManager.FindById(User.Identity.GetUserId());

            return View(model);
        }

        public ActionResult PlaceOrder(IEnumerable<SaveOrderRequest> products) {
            JsonResult result = new JsonResult {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

            var boughtProducts = productsService.GetProducts(products.Select(x => x.Id));

            Order newOrder = new Order {
                UserId = User.Identity.GetUserId(),
                OrderedAt = DateTime.Now,
                Status = "Pending",
                TotalAmount = boughtProducts.Sum(x => x.Price * products.FirstOrDefault(y => y.Id == x.Id)?.Quantity ?? 0),

                Items = boughtProducts.Select(x => new OrderItem() {
                    ProductId = x.Id,
                    Quantity = products.FirstOrDefault(y => y.Id == x.Id)?.Quantity ?? 0,
                }).ToList(),
            };

            var rowsEffected = shopService.SaveOrder(newOrder);

            result.Data = new { Success = rowsEffected > 0, Rows = rowsEffected };

            return result;
        }

    }
}