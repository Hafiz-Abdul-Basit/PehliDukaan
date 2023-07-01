//using PehliDukaan.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using PehliDukaan.Entities;
using PehliDukaan.Services;
using PehliDukaan.Services.Models;
using PehliDukaan.Services.Models.Requests;
using PehliDukaan.web.Models.ViewModels;
using PehliDukaan.Services.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Threading.Tasks;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using PehliDukaan.web.Models;

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

            // Get the cart products from cookies
            string cartProductsCookie = HttpUtility.UrlDecode(Request.Cookies["CartProducts"].Value);
            IEnumerable<ProductCartCookie> cartItems = JsonConvert.DeserializeObject<IEnumerable<ProductCartCookie>>(cartProductsCookie);

            // Get the user ID
            var userId = User.Identity.GetUserId();

            // Add or update cart products in the database
            cartService.AddOrUpdateProductsAsync(cartItems, userId).Wait();

            if (cartItems != null && cartItems.Any()) {
                model.CartProducts = productsService.GetCartProducts(cartItems);
                model.User = UserManager.FindById(User.Identity.GetUserId());
            }

            return View(model);
        }

        // GET: Shop
        [Authorize]
        public ActionResult Checkout() {
            CheckoutViewModel model = new CheckoutViewModel();

            string CartProductsCookie = HttpUtility.UrlDecode(Request.Cookies["CartProducts"].Value);
            IEnumerable<ProductCartCookie> cartItems = JsonConvert.DeserializeObject<IEnumerable<ProductCartCookie>>(CartProductsCookie);

            if (cartItems == null || cartItems.Any() == false) {
                return View(model);
            }

            model.CartProducts = productsService.GetCartProducts(cartItems);
            model.User = UserManager.FindById(User.Identity.GetUserId());
         
            return View(model);
        }
        CartService cartService = new CartService();
        InvoiceService invoiceService = new InvoiceService();
        EmailResponse emailResponse  = new EmailResponse();
        [HttpPost]
        public async Task<ActionResult> PlaceOrderAsync(IEnumerable<SaveOrderRequest> products) {
            CheckoutViewModel model = new CheckoutViewModel();

            JsonResult result = new JsonResult {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

            // Retrieve the user details from the model
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            var boughtProducts = productsService.GetProducts(products.Select(x => x.Id));

            Order newOrder = new Order {
                UserId = User.Identity.GetUserId(),
                OrderedAt = DateTime.Now,
                Status = "In Progress",
                TotalAmount = boughtProducts.Sum(x => x.Price * products.FirstOrDefault(y => y.Id == x.Id)?.Quantity ?? 0),

                Items = boughtProducts.Select(x => new OrderItem() {
                    ProductId = x.Id,
                    Quantity = products.FirstOrDefault(y => y.Id == x.Id)?.Quantity ?? 0,
                }).ToList(),
            };

            // Get the user's email
            var userEmail = user.Email;

            // Send the order confirmation email

            var rowsAffected = shopService.SaveOrder(newOrder);

            if (rowsAffected > 0) {


                // Generate the invoice report
                var orderInvoiceBytes = invoiceService.GenerateInvoiceReport(newOrder);

                // Get the product image

                // Send the invoice report and product image as email attachments to the user
                emailResponse.SendInvoiceReportEmail(orderInvoiceBytes, newOrder, userEmail);

                // Remove cart items from the database
                await cartService.RemoveCartItemsAsync(userId);

                result.Data = new { Success = true, Rows = rowsAffected };
            }

            result.Data = new { Success = rowsAffected > 0, Rows = rowsAffected };

            return result;
        }



    }
}