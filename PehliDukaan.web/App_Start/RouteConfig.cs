using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PehliDukaan.web {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

          
            routes.MapRoute(
              name: "SearchingProduct",
              url: "Search/AllProducts",
              defaults: new { controller = "Product", action = "ProductTable", id = UrlParameter.Optional }
          );

            routes.MapRoute(
             name: "CreateProduct",
             url: "Product/Create",
             defaults: new { controller = "Product", action = "Create", id = UrlParameter.Optional }
         );
            routes.MapRoute(
          name: "EditProduct",
          url: "Product/Edit",
          defaults: new { controller = "Product", action = "Edit", id = UrlParameter.Optional }
      );

            routes.MapRoute(
           name: "DeleteProduct",
           url: "Product/Delete",
           defaults: new { controller = "Product", action = "Delete", id = UrlParameter.Optional }
       );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
