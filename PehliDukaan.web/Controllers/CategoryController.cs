using PehliDukaan.Services;
using PehliDukaan.Entities;
using PehliDukaan.web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MailKit.Search;
using System.Globalization;
using Microsoft.IdentityModel.Tokens;
using System.IO;

namespace PehliDukaan.web.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        CategoriesService categoryService = new CategoriesService();

        [Authorize(Roles = "Administrator")]
        public ActionResult Index() {
         
            return View();
        }


        public ActionResult CategoryList() {
            ShopViewModel model = new ShopViewModel();

            model.FeaturedCategories = categoryService.GetFeaturedCategories();
      
            return View(model);
            
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult CategoryTable(string search) {

            CategorySearchViewModel model = new CategorySearchViewModel();

            
            model.Categories = categoryService.GetCategories();
            if (!string.IsNullOrEmpty(search)) {

                model.SearchItem = search;
                model.Categories = model.Categories.Where(p => p.Name != null && p.Name.ToLower().Contains(search.ToLower())).ToList();

            }

            return PartialView("CategoryTable", model);
        }

        [Authorize(Roles = "Administrator")]

        // Create: Category
        public ActionResult Create()
        {
            NewCategoryViewModel model = new NewCategoryViewModel();
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Create(NewCategoryViewModel model) {
            try {
                if (ModelState.IsValid) {
                    var newCategory = new Category();
                    newCategory.Name = model.Name;
                    newCategory.Description = model.Description;
                    newCategory.isFeatured = model.isFeatured;
                    newCategory.ImageData = model.ImageData;

                    // Save the category in the database
                    categoryService.SaveCategory(newCategory);

                    return RedirectToAction("CategoryTable");
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


        // Edit: Category
        [Authorize(Roles = "Administrator")]

        public ActionResult Edit(int Id) {


            EditCategoryViewModel model = new EditCategoryViewModel();


            //var Category = categoryService.GetCategory(Id);
            var Category = categoryService.GetCategory(Id);

            model.Id = Category.Id;
            model.Name = Category.Name;
            model.Description = Category.Description;
            model.ImageData = Category.ImageData;
            model.isFeatured = Category.isFeatured;

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Edit(EditCategoryViewModel model) {
            try {
                if (ModelState.IsValid) {
                    var existingCategory = categoryService.GetCategory(model.Id);
                    existingCategory.Name = model.Name;
                    existingCategory.Description = model.Description;
                    existingCategory.isFeatured = model.isFeatured;

                    // Update image only if it's provided
                    if (model.Image != null && model.Image.ContentLength > 0) {
                        using (var memoryStream = new MemoryStream()) {
                            model.Image.InputStream.CopyTo(memoryStream);
                            existingCategory.ImageData = memoryStream.ToArray();
                        }
                    }

                    categoryService.UpdateCategory(existingCategory);

                    return RedirectToAction("CategoryTable");
                }
                else {
                    return new HttpStatusCodeResult(400, "Invalid data");
                }
            }
            catch (Exception ex) {
                return new HttpStatusCodeResult(500, "An error occurred: " + ex.Message);
            }
        }



        // Delete: Category
        [HttpPost]
        public ActionResult Delete(int Id) {
            categoryService.DeleteCategory(Id);

            return RedirectToAction("CategoryTable");
        }
    }
}
  