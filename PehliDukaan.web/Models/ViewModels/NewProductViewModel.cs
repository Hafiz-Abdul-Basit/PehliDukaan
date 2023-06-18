using PehliDukaan.Entities;
using PehliDukaan.Services.Models.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PehliDukaan.web.Models.ViewModels {

    public class ProductSearchViewModel {

        public int PageNo { get; set; }
        public List<Product> Products { get; set; }
        public string SearchItem { get; set; }
        public Pager Pager { get; set; }

    }

    public class NewProductViewModel {

        [Required]
        [MinLength(5), MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }

        public string ImageURL { get; set; }
        public List<Category> AvailableCategories { get; set; }
    }

    public class EditProductViewModel {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; } 
        public decimal Price { get; set; }

        public string ImageURL { get; set; }
        public List<Category> AvailableCategories { get; set; }
    }

    public class ProductViewModel {
        public Product Product { get; set; }
        public int Quantity { get; set; }

    }
}