using PehliDukaan.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace PehliDukaan.Database.DataService {
    public class CategoriesService {

        //public static CategoriesService ClassObject {

        //    get {
        //        if (privateInMemoryObject == null) privateInMemoryObject = new CategoriesService();
        //        return privateInMemoryObject;
        //    }


        //}
        //private static CategoriesService privateInMemoryObject { get; set; }

        //private CategoriesService() {


        //}


        public Category GetCategory(int Id) {

            using (var context = new PDContext()) {
                return context.Categories.Find(Id);
            }
        }

        public List<Category> GetCategories() {

            using (var context = new PDContext()) {
                return context.Categories.Include(x => x.Products).ToList();
            }
            //return new List<Category> { GetCategory(0) };
        }

        public List<Category> GetFeaturedCategories() {

            using (var context = new PDContext()) {
                return context.Categories.Where(x => x.isFeatured && x.ImageURL != null).ToList();
            }
        }

        public void SaveCategory(Category category) {

            using (var context = new PDContext()) {
                context.Categories.Add(category);
                context.SaveChanges();
            }
        }

        public void UpdateCategory(Category category) {

            using (var context = new PDContext()) {

                context.Entry(category).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteCategory(int Id) {

            using (var context = new PDContext()) {

                var category = context.Categories.Where(x => x.Id == Id).Include(x => x.Products).FirstOrDefault();

                context.Products.RemoveRange(category.Products); //first delete products of this category
                context.Categories.Remove(category);
                context.SaveChanges();
            }
        }

    }
}
