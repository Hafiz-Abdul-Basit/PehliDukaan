using PehliDukaan.Entities;
using PehliDukaan.Services;
using PehliDukaan.web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PehliDukaan.web.Controllers
{
    public class ProductController : Controller
    {
        ProductsService productsService = new ProductsService();

        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProductTable(string search)
        {
              var products = productsService.GetProducts();
              
              if (string.IsNullOrEmpty(search) == false) {

              products = products.Where(p => p.Name !=null && p.Name.ToLower().Contains(search.ToLower())).ToList();
              
            }
              
              return PartialView(products);
        }

        // GET: Category
        public ActionResult Create() {

            CategoriesService categoriesService = new CategoriesService();

            var categories = categoriesService.GetCategories();
            return PartialView(categories);
        }

        [HttpPost]
        public ActionResult Create(Product product) {

            productsService.SaveProduct(product);

            return RedirectToAction("ProductTable");
        }

        // GET: Category
        public ActionResult Edit(int Id) {

            var product = productsService.GetProduct(Id);

            return PartialView(product);
        }

        [HttpPost]
        public ActionResult Edit(NewCategoryViewModel model) {

            CategoriesService categoriesService = new CategoriesService();

            var newProduct = new Product();
            newProduct.Name = model.Name;   
            newProduct.Description = model.Description;
            newProduct.Price = model.Price;
            newProduct.Category = categoriesService.GetCategory(model.CategoryId);

            productsService.UpdateProduct(newProduct);

            return RedirectToAction("ProductTable");
        }


        [HttpPost]
        public ActionResult Delete(int Id) {

            productsService.DeleteProduct(Id);

            return RedirectToAction("ProductTable");
        }
    }
}