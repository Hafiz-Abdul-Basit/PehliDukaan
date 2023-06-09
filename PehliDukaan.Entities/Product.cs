﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PehliDukaan.Entities {

    public class Product : BaseEntity { 

        public virtual Category Category { get; set; }

        //public int CategoryId { get; set; } 

        [Range(1, 100000)]
        public decimal Price { get; set; }

        public int CategoryID { get; set; }

        public byte[] ImageData { get; set; }
    }
}
