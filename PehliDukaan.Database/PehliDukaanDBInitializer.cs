using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PehliDukaan.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PehliDukaan.Database {
    public class PehliDukaanDBInitializer : CreateDatabaseIfNotExists<PDContext> {
        protected override void Seed(PDContext context) {

            SeedRoles(context);
            SeedUsers(context);
        }


        public void SeedRoles(PDContext context) {

            List<IdentityRole> rolesInShop = new List<IdentityRole>();

            rolesInShop.Add(new IdentityRole() { Name = "Administrator" });
            rolesInShop.Add(new IdentityRole() { Name = "Moderator" });
            rolesInShop.Add(new IdentityRole() { Name = "User" });

            var roleStore = new RoleStore<IdentityRole>(context);
            var rolesManager = new RoleManager<IdentityRole>(roleStore);

            foreach (IdentityRole role in rolesInShop) {

                if (!rolesManager.RoleExists(role.Name)) {

                    var result = rolesManager.Create(role);

                    if (result.Succeeded) continue;
                }
            }

        }

        public void SeedUsers(PDContext context) {

            var usersStore = new UserStore<PehliDukaanUser>(context);
            var usersManager = new UserManager<PehliDukaanUser>(usersStore);

            PehliDukaanUser admin = new PehliDukaanUser();
            admin.Email = "admin@email.com";
            admin.UserName = "admin";
            var password = "admin123";

            if (usersManager.FindByEmail(admin.Email) == null) {

                var result = usersManager.Create(admin, password);

                if (result.Succeeded) {

                    //add necessary roles to admin
                    usersManager.AddToRole(admin.Id, "Administrator");
                    usersManager.AddToRole(admin.Id, "Moderator");
                    usersManager.AddToRole(admin.Id, "User");

                }

            }

        }

    }
}
