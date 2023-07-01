using PehliDukaan.Entities;
using PehliDukaan.Services.Models.Responses;
using System.Collections.Generic;
using System.Linq;

namespace PehliDukaan.web.Models.ViewModels {


    public class ShopViewModel {
        public int MaximumPrice { get; set; }
        public List<Category> FeaturedCategories { get; set; }
        public List<Product> Products { get; set; }
        public int? SortBy { get; set; }
        public int? CategoryID { get; set; }
        public string SearchTerm { get; set; }
    }


    public class CheckoutViewModel {

        public IEnumerable<CartProductResponse> CartProducts { get; set; } = Enumerable.Empty<CartProductResponse>();
        public PehliDukaanUser User { get; set; }



    }





    public class FilterProductsViewModel {
        public List<Product> Products { get; set; }

        public Pager Pager { get; set; }
        public int? SortBy { get; set; }
        public int? CategoryID { get; set; }
        public string SearchTerm { get; set; }
    }
}