using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PehliDukaan.Entities {

    public class BaseEntity {
        public int Id { get; set; }

        [Required]
        [Display]
        [MinLength(5), MaxLength(50)]
        public string Name { get; set; }


        [MaxLength(500)]
        public string Description { get; set; }
    }
}
