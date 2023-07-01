//using PehliDukaan.Services;
using PehliDukaan.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PehliDukaan.Services;
using PehliDukaan.Entities;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Net.Configuration;
using PehliDukaan.web.Models.ViewModels;
using System.IO;
using System.Threading.Tasks;

namespace PehliDukaan.web.Controllers {

    public class HomeController : Controller {

        CategoriesService categoryService = new CategoriesService();
        ProductsService productsService = new ProductsService();

        public ActionResult Index(int? categoryID) {

            HomeViewModel model = new HomeViewModel();

            model.FeaturedCategories = categoryService.GetFeaturedCategories();
            return View(model);
        }

        public ActionResult About() {
            ViewBag.Message = "Your application description page.";

            return View();
        }


        // GET: Home
        [HttpGet]
        public ActionResult ContactForm() {
            return View();
        }

        [HttpPost]

        public ActionResult ContactForm(string email, string subject, string message) {
            try {
                string host = "smtp.gmail.com";
                string password = "pflsdtshrayxhaqo\r\n";
                int port = 587;
                bool enableSsl = true;

                string senderEmail = "abdul.basitttt8@gmail.com";
                string displayName = "PehliDUKAAN";

                var sender = new MailAddress(senderEmail, displayName);

                string receiverEmail = email;
                string receiverDisplayName = "PehliDukaan";

                var receiver = new MailAddress(receiverEmail, receiverDisplayName);

                message += Environment.NewLine + $"Sender: {email}";



                if (ModelState.IsValid) {
                    var smtp = new SmtpClient {
                        Host = host,
                        Port = port,
                        EnableSsl = enableSsl,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail, password)
                    };

                    using (var mess = new MailMessage(senderEmail, receiverEmail) {
                        Subject = subject,
                        Body = message
                    }) {
                        smtp.Send(mess);
                    }

                    return RedirectToAction("Index"); // Redirect to a success page or desired action
                }
            }
            catch (Exception ex) {
                ViewBag.Error = "Some Error";
                Response.Write(ex.ToString());
            }

            return View();
        }
    }
}