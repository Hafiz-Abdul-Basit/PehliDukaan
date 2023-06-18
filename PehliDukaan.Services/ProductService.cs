using System;
using PehliDukaan.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using PehliDukaan.Database;
using PehliDukaan.Services.Models.Responses;
using PehliDukaan.Services.Models;

namespace PehliDukaan.Services {
    public class ProductsService {

        #region Singleton
        //public static ProductsService ClassObject {

        //    get {
        //        if (privateInMemoryObject == null) privateInMemoryObject = new ProductsService();
        //        return privateInMemoryObject;
        //    }


        //}
        //private static ProductsService privateInMemoryObject { get; set; }

        //private ProductsService() {


        //}

        public List<Product> SearchProducts(string searchTerm, int? minimumPrice, int? maximumPrice, int? categoryID, int? sortBy) {
            using (var context = new PDContext()) {
                var products = context.Products.ToList();

                if (categoryID.HasValue) {
                    products = products.Where(x => x.Category.Id == categoryID.Value).ToList();
                }

                if (!string.IsNullOrEmpty(searchTerm)) {
                    products = products.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
                }

                if (minimumPrice.HasValue) {
                    products = products.Where(x => x.Price >= minimumPrice.Value).ToList();
                }

                if (maximumPrice.HasValue) {
                    products = products.Where(x => x.Price <= maximumPrice.Value).ToList();
                }

                if (sortBy.HasValue) {
                    switch (sortBy.Value) {
                        case 2:
                            products = products.OrderByDescending(x => x.Id).ToList();
                            break;
                        case 3:
                            products = products.OrderBy(x => x.Price).ToList();
                            break;
                        default:
                            products = products.OrderByDescending(x => x.Price).ToList();
                            break;
                    }
                }

                return products.ToList();
            }
        }


        public int GetMaximumPrice() {
            using (var context = new PDContext()) {
                return (int)(context.Products.Max(x => x.Price));
            }
        }

        #endregion

        public Product GetProduct(int Id) {

            using (var context = new PDContext()) {
                return context.Products.Where(x => x.Id == Id).Include(x => x.Category).FirstOrDefault();
            }
        }

        public IEnumerable<Product> GetProducts(IEnumerable<int> IDs) {

            using (var context = new PDContext()) {
                return context.Products.Where(product => IDs.Contains(product.Id)).ToList();
            }
        }
        public IEnumerable<CartProductResponse> GetCartProducts(IEnumerable<ProductCartCookie> cartItems) {

            using (var context = new PDContext()) {
                var ids = cartItems.Select(x => x.Id);
                var entities = context.Products.Where(product => ids.Contains(product.Id)).ToList();
                return entities.Select(x => new Models.Responses.CartProductResponse() {
                    CategoryID = x.CategoryID,
                    ImageURL = x.ImageURL,
                    Price = x.Price,
                    Id = x.Id,
                    Description = x.Description,
                    Name = x.Name,
                    Quantity = cartItems.First(y => y.Id == x.Id).Quantity,
                });
            }

        }
        public List<Product> GetProducts() {
           

            using (var context = new PDContext()) {

                return context.Products.Include(x => x.Category).ToList();

                //return context.Products.OrderBy(x => x.Id).Skip((pageNo - 1) * pageSize).Take(pageSize).Include(x => x.Category).ToList();

            }
        }


        public List<Product> GetProducts(int pageNo, int pageSize) {
            using (var context = new PDContext()) {
                return context.Products.OrderByDescending(x => x.Id).Skip((pageNo - 1) * pageSize).Take(pageSize).Include(x => x.Category).ToList();
            }
        }

        public List<Product> GetLatestProducts(int numberOfProducts) {
            using (var context = new PDContext()) {
                return context.Products.OrderByDescending(x => x.Id).Take(numberOfProducts).Include(x => x.Category).ToList();
            }
        }

        public List<Product> GetProductsByCategory(int categoryID, int pageSize) {
            using (var context = new PDContext()) {
                return context.Products.Where(x => x.Category.Id == categoryID).OrderByDescending(x => x.Id).Take(pageSize).Include(x => x.Category).ToList();
            }
        }

        public void SaveProduct(Product product) {

            using (var context = new PDContext()) {
                context.Entry(product.Category).State = System.Data.Entity.EntityState.Unchanged;

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
