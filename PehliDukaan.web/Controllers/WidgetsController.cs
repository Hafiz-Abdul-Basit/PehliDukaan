using PehliDukaan.Services;
using PehliDukaan.web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PehliDukaan.web.Controllers
{
    public class WidgetsController : Controller
    {

        ProductsService productsService = new ProductsService();
        // GET: Widgets
        public ActionResult Products(bool isLatestProducts, int? CategoryID = 0) {
            ProductsWidgetViewModel model = new ProductsWidgetViewModel();

            model.IsLatestProducts = isLatestProducts;

            if (isLatestProducts) {
                model.Products = productsService.GetLatestProducts(4).ToList();
            }
            else if (CategoryID.HasValue && CategoryID.Value > 0) {
                model.Products = productsService.GetProductsByCategory(CategoryID.Value, 4).ToList();
            }
            else {
                model.Products = productsService.GetProducts(1, 8).ToList();
            }

            return PartialView(model);
        }
    }
}