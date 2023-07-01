using MailKit.Security;
using MimeKit;
using PehliDukaan.Entities;
using PehliDukaan.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Services.Description;
using MailKit.Net.Smtp;


namespace PehliDukaan.web.Controllers {
    public class SharedController : Controller {

        // GET: Shared
        [HttpPost]
        public JsonResult UploadImage(HttpPostedFileBase Image) {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            try {
                if (Image != null && Image.ContentLength > 0) {
                    using (var memoryStream = new MemoryStream()) {
                        Image.InputStream.CopyTo(memoryStream);
                        var imageData = memoryStream.ToArray();
                        var base64Image = Convert.ToBase64String(imageData);
                        var imageSrc = $"{base64Image}";

                        result.Data = new { Success = true, ImageData = imageSrc };
                    }
                }
                else {
                    result.Data = new { Success = false, Message = "No image file was provided." };
                }
            }
            catch (Exception ex) {
                result.Data = new { Success = false, Message = ex.Message };
            }

            return result;
        }
    }
}