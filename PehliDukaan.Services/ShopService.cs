using PehliDukaan.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PehliDukaan.Database;


namespace PehliDukaan.Services {
    public class ShopService {

        public int SaveOrder(Order order) {
            using (var context = new PDContext()) {
                context.Orders.Add(order);
                return context.SaveChanges();
            }
        }
    }
}
