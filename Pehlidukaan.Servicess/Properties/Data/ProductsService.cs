using PehliDukaan.Database;
using PehliDukaan.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PehliDukaan.web.Data;

namespace PehliDukaan.Services {
    public class ProductsService {

        #region Singleton
        public static ProductsService ClassObject {

            get {
                if (privateInMemoryObject == null) privateInMemoryObject = new ProductsService();
                return privateInMemoryObject;
            }


        }
        private static ProductsService privateInMemoryObject { get; set; }

        private ProductsService() {


        }

        #endregion

        public Product GetProduct(int Id) {

            using (var context = new PDContext()) {
                return context.Products.Where(x=>x.Id == Id).Include(x => x.Category).FirstOrDefault();
            }
        }

        public List<Product> GetProducts(List<int> IDs) {

            using (var context = new PDContext()) {
                return context.Products.Where(product => IDs.Contains(product.Id)).ToList();
            }
        }

        public List<Product> GetProducts(int pageNo) {


            using (var context = new PDContext()) {
                return context.Products.Include(x => x.Category).ToList();
            }
        }

        public void SaveProduct(Product product) { 

            using (var context = new PDContext())
            {
                context.Entry(product).State = EntityState.Unchanged;

                context.Products.Add(product);
                context.SaveChanges();
            }
        }

        public void UpdateProduct(Product product) {

            using (var context = new PDContext()) {

                context.Entry(product).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteProduct(int Id) {

            using (var context = new PDContext()) {

                var product = context.Products.Find(Id);
                context.Products.Remove(product);
                context.SaveChanges();
            }
        }



    }
}
