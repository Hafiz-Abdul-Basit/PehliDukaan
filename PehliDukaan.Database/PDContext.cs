using PehliDukaan.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PehliDukaan.Database {
    public class PDContext : DbContext
    {
        public PDContext() : base("PehliDukaanConnection")
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

    }
}
