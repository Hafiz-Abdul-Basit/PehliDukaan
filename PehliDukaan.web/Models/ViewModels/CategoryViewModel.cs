using PehliDukaan.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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
        public string ImageURL { get; set; }
        public bool isFeatured { get; set; }



    }
    public class EditCategoryViewModel {

        public int Id { get; set; }

        //[Required]
        //[MinLength(5), MaxLength(50)]
        public string Name { get; set; }

        //[MaxLength(500)]
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public bool isFeatured { get; set; }


       
    }
}