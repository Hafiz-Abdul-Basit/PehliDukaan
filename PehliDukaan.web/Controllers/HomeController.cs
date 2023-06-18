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
                if (ModelState.IsValid) {
                    var senderEmail = new MailAddress(email, "Sender");
                    var receiverEmail = new MailAddress("abdul.basitttt73@gmail.com", "Admin");
                    var password = "abdulbasit1234";
                    var sub = subject;
                    var body = message;

                    var smtp = new SmtpClient {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };

                    using (var mess = new MailMessage(receiverEmail, senderEmail) {
                        Subject = sub,
                        Body = body
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