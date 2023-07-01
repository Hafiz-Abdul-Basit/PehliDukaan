using PehliDukaan.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PehliDukaan.web.Models.ViewModels {

    public class CategorySearchViewModel {

        public List<Category> Categories { get; set; }
        public string SearchItem { get; set; }

    }

    public class NewCategoryViewModel {


        [Required]
        [MinLength(5), MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public byte[] ImageData { get; set; }
        public bool isFeatured { get; set; }



    }
    public class EditCategoryViewModel {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a name.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Name must be between 5 and 50 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        public HttpPostedFileBase Image { get; set; }

        public byte[] ImageData { get; set; } 

        public bool isFeatured { get; set; }
    }
}