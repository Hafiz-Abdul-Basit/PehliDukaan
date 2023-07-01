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
    public class ReportService {
        private readonly OrderService orderService;

        public ReportService() {
            orderService = new OrderService();
        }

        public List<Order> GetOrdersByMonthAndProduct(int currentMonth, int currentYear) {
            var orders = orderService.GetOrdersByMonth(currentMonth, currentYear);

            return orders;
        }

        public List<ProductReportItem> GenerateProductReport(List<Order> orders) {
            var productReport = new List<ProductReportItem>();

            foreach (var order in orders) {
                foreach (var item in order.Items) {
                    var existingReportItem = productReport.FirstOrDefault(x => x.ProductId == item.ProductId);

                    if (existingReportItem != null) {
                        if (existingReportItem.QuantityByMonth.ContainsKey(order.OrderedAt.Month)) {
                            existingReportItem.QuantityByMonth[order.OrderedAt.Month] += item.Quantity;
                        }
                        else {
                            existingReportItem.QuantityByMonth[order.OrderedAt.Month] = item.Quantity;
                        }
                    }
                    else {
                        var product = item.Product;
                        var reportItem = new ProductReportItem {
                            ProductId = item.ProductId,
                            ProductName = product.Name,
                            QuantityByMonth = new Dictionary<int, int>
                            {
                        { order.OrderedAt.Month, item.Quantity }
                    }
                        };

                        productReport.Add(reportItem);
                    }
                }
            }

            return productReport;
        }
    }

    public class ProductReportItem {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public Dictionary<int, int> QuantityByMonth { get; set; }
    }
}
