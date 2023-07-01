using Microsoft.Reporting.WebForms;
using PehliDukaan.Database;
using PehliDukaan.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PehliDukaan.Services {
    public class InvoiceService {
        public byte[] GenerateInvoiceReport(Order order) {
            using (var context = new PDContext()) {
                // Fetch the required data using Entity Framework
                var orderItems = context.OrderItems
                    .Where(item => item.OrderId == order.Id)
                    .Include(item => item.Product)
                    .ToList();

                DataTable orderTable = new DataTable();
                orderTable.Columns.Add("Id", typeof(int));
                orderTable.Columns.Add("Name", typeof(string));
                orderTable.Columns.Add("Quantity", typeof(int));
                orderTable.Columns.Add("Price", typeof(decimal));
                orderTable.Columns.Add("TotalAmount", typeof(decimal));
                orderTable.Columns.Add("Status", typeof(string));
                orderTable.Columns.Add("OrderedAt", typeof(DateTime));

                foreach (var orderItem in orderItems) {
                    orderTable.Rows.Add(
                        order.Id,
                        orderItem.Product.Name,
                        orderItem.Quantity,
                        orderItem.Product.Price,
                        orderItem.Quantity * orderItem.Product.Price,
                        order.Status,
                        order.OrderedAt
                    );
                }

                var reportViewer = new ReportViewer();
                var reportPath = HttpContext.Current.Server.MapPath("~/Report/InvoiceReport.rdlc");
                reportViewer.LocalReport.ReportPath = reportPath;
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ReportData", orderTable));

                return reportViewer.LocalReport.Render("PDF");
            }
        }
    }
}
