﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Reporting.WebForms;
using PehliDukaan.Database;
using PehliDukaan.Entities;
using PehliDukaan.Services;
using PehliDukaan.Services.Models.Requests;
using PehliDukaan.web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PehliDukaan.web.Controllers {
    public class OrderController : Controller {
        private PDSignInManager _signInManager;
        private PDUserManager _userManager;

        public PDSignInManager SignInManager {
            get {
                return _signInManager ?? HttpContext.GetOwinContext().Get<PDSignInManager>();
            }
            private set {
                _signInManager = value;
            }
        }
        public PDUserManager UserManager {
            get {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<PDUserManager>();
            }
            private set {
                _userManager = value;
            }
        }

        OrderService orderService = new OrderService();
        // GET: Order
        public ActionResult Index(string userID, string status) {

            OrdersViewModel model = new OrdersViewModel();
            model.UserID = userID;
            model.Status = status;
            model.Orders = orderService.SearchOrders(userID, status);
            var totalRecords = orderService.SearchOrdersCount(userID, status);

            return View(model);
        }
        public ActionResult Details(int ID) {

            OrderDetailsViewModel model = new OrderDetailsViewModel();

            model.Order = orderService.GetOrderByID(ID);

            if (model.Order != null) {
                model.OrderBy = UserManager.FindById(model.Order.UserId);
            }

            model.AvailableStatuses = new List<string>() { "Pending", "In Progress", "Delivered" };

            return View(model);
        }

        public ActionResult UserProfileDetails() {

            string userId = User.Identity.GetUserId();
            var orders = orderService.GetOrdersByUserId(userId);
            var model = new OrdersViewModel {
                Orders = orders
            };

            return View(model);
        }

        public JsonResult ChangeStatus(string status, int ID) {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            result.Data = new { Success = orderService.UpdateOrderStatus(ID, status) };

            return result;
        }

        public ActionResult GenerateInvoice(int orderId) {
            OrderService orderService = new OrderService();
            var order = orderService.GetOrderByID(orderId);

            if (order != null) {

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
                    orderTable.Columns.Add("Price", typeof(decimal)); // Update column name
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
                        reportViewer.LocalReport.ReportPath = Server.MapPath("~/Report/InvoiceReport.rdlc");
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ReportData", orderTable));

                        var renderedBytes = reportViewer.LocalReport.Render("PDF");

                        // Clear the response and set the necessary headers for file download
                        Response.Clear();
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-disposition", "attachment;filename=Invoice.pdf");

                        // Write the renderedBytes to the response stream
                        Response.BinaryWrite(renderedBytes);
                        Response.Flush();
                        Response.End();
                    }
                
                
            }

            return HttpNotFound();
        }
    
    }
}
