using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using PehliDukaan.Entities;

namespace PehliDukaan.Services {
    public class PDSignInManager : SignInManager<PehliDukaanUser, string> {
        public PDSignInManager(PDUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager) {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(PehliDukaanUser user) {
            return user.GenerateUserIdentityAsync((PDUserManager)UserManager);
        }

        public static PDSignInManager Create(IdentityFactoryOptions<PDSignInManager> options, IOwinContext context) {
            return new PDSignInManager(context.GetUserManager<PDUserManager>(), context.Authentication);
        }
    }
}
