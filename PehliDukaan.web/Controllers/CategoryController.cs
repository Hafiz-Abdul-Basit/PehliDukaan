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
    public class CategoryController : Controller
    {
        CategoriesService categoryService = new CategoriesService();

        public ActionResult Index() {
         
            return View();
        }

        public ActionResult CategoryTable(string search) {

            CategorySearchViewModel model = new CategorySearchViewModel();
           
            model.Categories = categoryService.GetCategories();

            if (!string.IsNullOrEmpty(search)) {

                model.SearchItem = search;
                model.Categories = model.Categories.Where(p => p.Name != null && p.Name.ToLower().Contains(search.ToLower())).ToList();

            }

            return PartialView("CategoryTable", model);
        }


        // Create: Category
        public ActionResult Create()
        {
            NewCategoryViewModel model = new NewCategoryViewModel();
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Create(NewCategoryViewModel model) 
        {

            var newCategory = new Category();
            newCategory.Name = model.Name;
            newCategory.Description = model.Description;
            newCategory.ImageURL = model.ImageURL;
            newCategory.isFeatured = model.isFeatured;
            categoryService.SaveCategory(newCategory);

            return RedirectToAction("CategoryTable");

        }


        // Edit: Category
        public ActionResult Edit(int Id) {


            EditCategoryViewModel model = new EditCategoryViewModel();


            var Category = categoryService.GetCategory(Id);

            model.Id = Category.Id;
            model.Name = Category.Name;
            model.Description = Category.Description;
            model.ImageURL = Category.ImageURL;
            model.isFeatured = Category.isFeatured;

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Edit(EditCategoryViewModel model) {


            var existingCategory = categoryService.GetCategory(model.Id);
            existingCategory.Name = model.Name;
            existingCategory.Description = model.Description;
            existingCategory.ImageURL= model.ImageURL;
            existingCategory.isFeatured = model.isFeatured;

            categoryService.UpdateCategory(existingCategory);

            return RedirectToAction("CategoryTable");

        }

        // Delete: Category
        [HttpPost]
        public ActionResult Delete(int Id) {

            categoryService.DeleteCategory(Id);

            return RedirectToAction("CategoryTable");
        }
    }
}
  