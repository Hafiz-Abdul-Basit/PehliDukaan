using Microsoft.AspNet.Identity.EntityFramework;
using PehliDukaan.Entities;
using PehliDukaan.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PehliDukaan.web.Models.ViewModels {
    public class DashboardViewModel{

        public int CategoriesCount;
        public int ProductsCount;

        public int OrdersCount;
        public int TotalEarnings { get; set; }

    }

    public class UsersViewModel {

        public string SearchTerm { get; set; }

        public string RoleID { get; set; }
        public List<IdentityRole> Roles { get;  set; }
    }

    public class UsersListingViewModel {

        public string SearchTerm { get; set; }

        public string RoleID { get; set; }
        public List<PehliDukaanUser> Users { get; set; }


    }

    public class RolesListingViewModel {

        public string SearchTerm { get; set; }

        public List<IdentityRole> Roles { get; set; }


    }

    public class UserDetailsViewModel {

       public PehliDukaanUser User { get; set; }


    }

    public class UserRolesViewModel {

        public List<IdentityRole> AvailableRoles { get; set; }
        public List<IdentityRole> UserRoles { get; set; }
        public PehliDukaanUser User { get; internal set; }
    }

    public class RolesViewModel {

        public string SearchTerm{ get; set; }


    }
    public class ReportsViewModel {
        public int CurrentMonth { get; set; }
        public int CurrentYear { get; set; }
        public List<ProductReportItem> ProductReport { get; set; }

    }
}