using PehliDukaan.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PehliDukaan.web.Models {

    public class HomeViewModel {

        public List<Category> FeaturedCategories { get; set; }
        public List<Product> FeaturedProducts { get; set; }
        public int? CategoryID { get; set; }

        public int? SortBy { get; set; }

    }


}