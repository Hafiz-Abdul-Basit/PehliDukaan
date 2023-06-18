using PehliDukaan.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PehliDukaan.Database;
using System.Runtime.Remoting.Contexts;


namespace PehliDukaan.Services {
    public class OrderService {

        public List<Order> SearchOrders(string userID, string status) {
            using (var context = new PDContext()) {
                var orders = context.Orders.ToList();

                if (!string.IsNullOrEmpty(userID)) {
                    orders = orders.Where(x => x.UserId.ToLower().Contains(userID.ToLower())).ToList();
                }
              

                if (!string.IsNullOrEmpty(status)) {
                    orders = orders.Where(x => x.Status.ToLower().Contains(status.ToLower())).ToList();
                }

                return orders.ToList();
            }
        }
        public int SearchOrdersCount(string userID, string status) {
            using (var context = new PDContext()) {
                var orders = context.Orders.ToList();

                if (!string.IsNullOrEmpty(userID)) {
                    orders = orders.Where(x => x.UserId.ToLower().Contains(userID.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(status)) {
                    orders = orders.Where(x => x.Status.ToLower().Contains(status.ToLower())).ToList();
                }

                return orders.Count;
            }
        }

        public Order GetOrderByID(int ID) {
            using (var context = new PDContext()) {
                return context.Orders.Where(x => x.Id == ID).Include(x => x.Items).Include("Items.Product").FirstOrDefault();
            }
        }

        public bool UpdateOrderStatus(int ID, string status) {
            using (var context = new PDContext()) {
                var order = context.Orders.Find(ID);

                order.Status = status;

                context.Entry(order).State = EntityState.Modified;

                return context.SaveChanges() > 0;
            }
        }

        public List<Order> GetOrdersByUserId(string userId) {
          
            using (var context = new PDContext()) {
                return context.Orders.Where(o => o.UserId == userId).ToList(); ;
            }
        }

        public Order GetOrdersByID(int orderId) {
            using (var context = new PDContext()) {
                return context.Orders
                    .Include(o => o.Items) // Include the order items
                    .Include("Items.Product") // Include the associated product for each order item
                    .FirstOrDefault(o => o.Id == orderId);
            }
        }

        public List<Order> GetOrdersByMonth(int currentMonth, int currentYear) {
            using (var context = new PDContext()) {
                return context.Orders
                    .Where(o => o.OrderedAt.Month == currentMonth && o.OrderedAt.Year == currentYear)
                    .Include(o => o.Items)
                    .Include("Items.Product")
                    .ToList();
            }
        }
    }
}
