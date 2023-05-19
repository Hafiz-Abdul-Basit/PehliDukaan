using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PehliDukaan.Entities;
using PehliDukaan.Database;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace PehliDukaan.Services {

    public class PDRoleManager : RoleManager<IdentityRole> {

        public PDRoleManager(IRoleStore<IdentityRole, string> roleStore) : base(roleStore) 
        { 
        }

        public static PDRoleManager Create(IdentityFactoryOptions<PDRoleManager> options, IOwinContext context) {

            return new PDRoleManager(new RoleStore<IdentityRole>(context.Get<PDContext>()));
        }
    }
}
