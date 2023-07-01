using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using PehliDukaan.Entities;
using PehliDukaan.Services;
using PehliDukaan.Services.Models;
using PehliDukaan.Services.Models.Requests;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PehliDukaan.web.Controllers {
    public class CartController : Controller {
        private readonly CartService _cartService;

        public CartController()
        {
            _cartService = new CartService();
        }

        [HttpPost]
        public async Task<ActionResult> AddItems() {
            string requestBody = await new StreamReader(Request.InputStream).ReadToEndAsync();
            var userId = User.Identity.GetUserId();
            var request = JsonConvert.DeserializeObject<UpdateItemsRequest>(requestBody);

            bool isUpdated = await _cartService.AddOrUpdateProductsAsync(request.Products, userId);

            return isUpdated
                ? new HttpStatusCodeResult(HttpStatusCode.OK)
                : new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // PUT: Update Item(s) of cart
        [HttpPut]
        public async Task<ActionResult> UpdateItems() {
            string requestBody = await new StreamReader(Request.InputStream).ReadToEndAsync();
            var userId = User.Identity.GetUserId();
            var request = JsonConvert.DeserializeObject<UpdateItemsRequest>(requestBody);

            bool isUpdated = await _cartService.AddOrUpdateProductsAsync(request.Products, userId);

            return isUpdated
                ? new HttpStatusCodeResult(HttpStatusCode.OK)
                : new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: Get Items from cart
        [HttpGet]
        public ActionResult GetItems() {
            var userId = User.Identity.GetUserId();
            var cartItems = _cartService.GetCartItems(userId);

            // You can return the cart items in a suitable format (e.g., JSON, View) based on your requirements
            return Json(cartItems, JsonRequestBehavior.AllowGet);
        }

        // GET: Get Item from cart
        [HttpGet]
        public ActionResult GetItem(int id) {
            var userId = User.Identity.GetUserId();
            var cartItem = _cartService.GetCartItem(userId, id);

            if (cartItem == null) {
                return HttpNotFound(); // Return appropriate status code if item not found
            }

            return Json(cartItem, JsonRequestBehavior.AllowGet);
        }

        // DELETE: DELETE Item from cart
        [HttpDelete]
        public async Task<ActionResult> DeleteItem(int id) {
            var userId = User.Identity.GetUserId();
            bool isDeleted = await _cartService.DeleteCartProductAsync(userId, id);

            return isDeleted
                ? new HttpStatusCodeResult(HttpStatusCode.OK)
                : HttpNotFound(); // Return appropriate status code if item not found or not deleted
        }
    }
}