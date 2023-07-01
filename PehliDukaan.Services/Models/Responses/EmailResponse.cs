using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Utils;
using PehliDukaan.Database;
using PehliDukaan.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace PehliDukaan.Services.Models.Responses {
    public class EmailResponse {
        ProductsService productsService = new ProductsService();
        public void SendInvoiceReportEmail(byte[] reportBytes, Order order, string customerEmail) {
            // Load email settings from configuration
            string smtpServer = ConfigurationManager.AppSettings["Smtp:Server"];
            int smtpPort = int.Parse(ConfigurationManager.AppSettings["Smtp:Port"]);
            string smtpUsername = ConfigurationManager.AppSettings["Smtp:Username"];
            string smtpPassword = ConfigurationManager.AppSettings["Smtp:Password"];

            // Create the email message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("PehliDukaan", "a.basit.freelancer@gmail.com"));
            message.To.Add(new MailboxAddress("Customer", customerEmail));
            message.Subject = "Order Invoice";

            // Build the email body
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = "<h1>Order Invoice</h1>";
            bodyBuilder.HtmlBody += "<p>Thank you for your order. Please find the attached invoice report.</p>";

            // Attach the product image(s)
            foreach (var item in order.Items) {
                var product = productsService.GetProduct(item.ProductId);

                // Get the image data from product.ImageData
                byte[] imageData = product.ImageData;

                // Create the image attachment
                var imageAttachment = new MimePart("image", "jpeg") {
                    Content = new MimeContent(new MemoryStream(imageData), ContentEncoding.Default),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Inline),
                };

                // Set the Content-ID for referencing the image in HTML
                var contentId = MimeUtils.GenerateMessageId();
                imageAttachment.ContentId = contentId;

                // Append product details with embedded image
                bodyBuilder.HtmlBody += "<li>";
                bodyBuilder.HtmlBody += $"<img src=\"cid:{contentId}\" alt=\"Product Image\" width=\"150\" height=\"150\" /><br/>";
                bodyBuilder.HtmlBody += $"{product.Name} - {item.Quantity}";
                bodyBuilder.HtmlBody += "</li>";

                // Add the image attachment to the email
                bodyBuilder.LinkedResources.Add(imageAttachment);
            }

            // Attach the invoice report
            bodyBuilder.Attachments.Add("Invoice.pdf", reportBytes, ContentType.Parse("application/pdf"));

            message.Body = bodyBuilder.ToMessageBody();

            // Send the email using MailKit
            using (var client = new MailKit.Net.Smtp.SmtpClient()) {
                // Set callback to ignore certificate errors
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                // Connect and send email
                client.Connect(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                client.Authenticate(smtpUsername, smtpPassword);
                client.Send(message);

                client.Disconnect(true);
            }
        }

    }
}
