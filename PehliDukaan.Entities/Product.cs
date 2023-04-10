using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PehliDukaan.Entities {

    public class Product : BaseEntity { 

        public virtual Category Category { get; set; }

        //public int CategoryId { get; set; } 
        public decimal Price { get; set; }

    }
}
