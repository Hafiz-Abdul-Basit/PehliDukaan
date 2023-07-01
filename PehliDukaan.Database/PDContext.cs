using Microsoft.AspNet.Identity.EntityFramework;
using PehliDukaan.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PehliDukaan.Database {

    public class PDContext : IdentityDbContext<PehliDukaanUser> ,IDisposable {

        public PDContext() : base("PehliDukaanConnection")
        {
            System.Data.Entity.Database.SetInitializer<PDContext>(new PehliDukaanDBInitializer());
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Config> Configurations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<CartProduct> CartProducts { get; set; }
        public static PDContext Create() {
            return new PDContext();
        }
    }
}
