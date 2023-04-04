using PehliDukaan.Entities;
 using PehliDukaan.Services;
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

            var categories = categoryService.GetCategories();
         
            return View(categories);
        }


        // Create: Category
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category category) 
        {
            categoryService.SaveCategory(category);
            return RedirectToAction("Index");
        }


        // Edit: Category
        public ActionResult Edit(int Id) {

            var category = categoryService.GetCategory(Id);
            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(Category category) {

            categoryService.UpdateCategory(category);
            return RedirectToAction("Index");

        }

        // Delete: Category
        public ActionResult Delete(int Id) {

            var category = categoryService.GetCategory(Id);

            return View(category);
        }

        [HttpPost]
        public ActionResult Delete(Category category) {

            categoryService.DeleteCategory(category.Id);
            return RedirectToAction("Index");

        }
    }
}
  