using PehliDukaan.Database;
using PehliDukaan.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PehliDukaan.Services {
    public class ProductsService {


        public Product GetProduct(int Id) {

            using (var context = new PDContext()) {
                return context.Products.Find(Id);
            }
        }

        public List<Product> GetProducts() {

            using (var context = new PDContext()) {
                return context.Products.ToList();
            }
        }

        public void SaveCategory(Product product) { 

            using (var context = new PDContext())
            {
                context.Products.Add(product);
                context.SaveChanges();
            }
        }

        public void UpdateProduct(Product product) {

            using (var context = new PDContext()) {

                context.Entry(product).State = System.Data.Entity.EntityState.Modified;
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
