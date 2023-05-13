using PehliDukaan.Entities;
using PehliDukaan.Services.Models.Responses;
using System.Collections.Generic;

namespace PehliDukaan.web.Models.ViewModels {

    public class CheckoutViewModel {

        public IEnumerable<CartProductResponse> CartProducts { get; set; }


        public ApplicationUser User { get; set; }


    }

    public class ShopViewModel {
        public int MaximumPrice { get; set; }
        public IEnumerable<Category> FeaturedCategories { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public int? SortBy { get; set; }
        public int? CategoryID { get; set; }
        public string SearchTerm { get; set; }
    }



    //public class FilterProductsViewModel {
    //    public List<Product> Products { get; set; }

    //    public Pager Pager { get; set; }
    //    public int? SortBy { get; set; }
    //    public int? CategoryID { get; set; }
    //    public string SearchTerm { get; set; }
    //}
}