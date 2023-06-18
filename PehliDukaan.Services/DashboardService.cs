using PehliDukaan.Database;
using PehliDukaan.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PehliDukaan.Services {
    public class DashboardService {

        public int GetCategoriesCount() {
            PDContext context = new PDContext();
            return context.Categories.Count();
        }

        public int GetProductsCount() {
            PDContext context = new PDContext();
            return context.Products.Count();
        }

        public int GetOrdersCount() {
            PDContext context = new PDContext();
            return context.Orders.Count();
        }

        public int GetTotalEarnings() {
            PDContext context = new PDContext();
            return (int)context.Orders.Sum(o => o.TotalAmount);
        }


     
    }
}
