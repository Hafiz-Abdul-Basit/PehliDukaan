using PehliDukaan.Entities;
using PehliDukaan.web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PehliDukaan.Services;
using PehliDukaan.Services.Models;
using Newtonsoft.Json;
using System.IO;

namespace PehliDukaan.web.Controllers {
    public class ProductController : Controller {
        ProductsService productsService = new ProductsService();
        CategoriesService categoryService = new CategoriesService();

        // GET: Product
        [Authorize(Roles = "Administrator")]

        public ActionResult Index() {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult ProductTable(string search) {

            ProductSearchViewModel model = new ProductSearchViewModel();
            model.Products = productsService.GetProducts();

            if (string.IsNullOrEmpty(search) == false) {

                model.SearchItem = search;
                model.Products = model.Products.Where(p => p.Name != null && p.Name.ToLower().Contains(search.ToLower())).ToList();

            }

            return PartialView(model);
        }

        // GET: Category
        [Authorize(Roles = "Administrator")]
        public ActionResult Create() {

            NewProductViewModel model = new NewProductViewModel();

            model.AvailableCategories = categoryService.GetCategories();
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Create(NewProductViewModel model) {
            try {
                if (ModelState.IsValid) {
                    var newProduct = new Product();
                    newProduct.Name = model.Name;
                    newProduct.Description = model.Description;
                    newProduct.Price = model.Price;
                    newProduct.Category = categoryService.GetCategory(model.CategoryId);
                    newProduct.ImageData = model.ImageData;

                    // Save the category in the database
                    productsService.SaveProduct(newProduct);
                    return RedirectToAction("ProductTable");
                }
                else {
                    return new HttpStatusCodeResult(400, "Invalid data");
                }
            }
            catch (Exception ex) {
                // Log the exception for further investigation
                // and return an appropriate error response.
                return new HttpStatusCodeResult(500, "An error occurred: " + ex.Message);
            }
        }

        // GET: Category
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int Id) {

            EditProductViewModel model = new EditProductViewModel();


            var product = productsService.GetProduct(Id);

            model.Id = product.Id;
            model.Name = product.Name;
            model.Description = product.Description;
            model.Price = product.Price;
            model.CategoryId = product.Category != null ? product.Category.Id : 0;
            model.ImageData = product.ImageData;

            model.AvailableCategories = categoryService.GetCategories();
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Edit(EditProductViewModel model) {


            try {
                if (ModelState.IsValid) {
                    var existingProduct = productsService.GetProduct(model.Id);
                    existingProduct.Name = model.Name;
                    existingProduct.Description = model.Description;
                    existingProduct.Price = model.Price;

                    existingProduct.Category = null; //mark it null. Because the reference key is changed below
                    existingProduct.CategoryID = model.CategoryId;

                    // Update image only if it's provided
                    if (model.Image != null && model.Image.ContentLength > 0) {
                        using (var memoryStream = new MemoryStream()) {
                            model.Image.InputStream.CopyTo(memoryStream);
                            existingProduct.ImageData = memoryStream.ToArray();
                        }
                    }

                    productsService.UpdateProduct(existingProduct);

                    return RedirectToAction("ProductTable");
                }
                else {
                    return new HttpStatusCodeResult(400, "Invalid data");
                }
            }
            catch (Exception ex) {
                return new HttpStatusCodeResult(500, "An error occurred: " + ex.Message);
            }
        }


        [HttpPost]
        public ActionResult Delete(int Id) {

            productsService.DeleteProduct(Id);

            return RedirectToAction("ProductTable");
        }

        [HttpGet]

        public ActionResult Details(int ID) {
            ProductViewModel model = new ProductViewModel();


            model.Product = productsService.GetProduct(ID);


            if (model.Product == null) return HttpNotFound();

            return View(model);
        }
    }
}