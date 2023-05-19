using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PehliDukaan.Database;
using PehliDukaan.Entities;

namespace PehliDukaan.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class PehliDukaanUser : IdentityUser
    {
        public string Name { get; set; }        
        public string Address { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<PehliDukaanUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class PDContext : IdentityDbContext<PehliDukaanUser>

    {
        public PDContext()
            : base("PehliDukaanConnection", throwIfV1Schema: false)
        {
        }

        public static PDContext Create()
        {
            return new PDContext();
        }
    }
}