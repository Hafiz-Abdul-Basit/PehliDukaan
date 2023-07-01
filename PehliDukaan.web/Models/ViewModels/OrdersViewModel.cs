using PehliDukaan.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PehliDukaan.web.Models.ViewModels {
    public class OrdersViewModel {
        public List<Order> Orders { get; set; }
        public Pager Pager { get; set; }
        public string Status { get; set; }
        public string UserID { get; set; }
        public PehliDukaanUser OrderBy { get; set; }

    }


    public class OrderDetailsViewModel {
        public List<string> AvailableStatuses { get; set; }
        public Order Order { get; set; }
        public PehliDukaanUser OrderBy { get; set; }
        public PehliDukaanUser UpdatedUser { get; set; } // Added property

    }


    public class OrderConfirmationViewModel {
        public Order Order { get; set; }

    }
}