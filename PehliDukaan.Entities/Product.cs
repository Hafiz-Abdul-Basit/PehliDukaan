using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PehliDukaan.Entities {

    public class Product : BaseEntity {

        public decimal Price { get; set; }
        public Category Category { get; set; }

    }
}
