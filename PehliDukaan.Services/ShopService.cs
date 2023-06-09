﻿using PehliDukaan.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PehliDukaan.Database;
using System.Data.Entity;
using PehliDukaan.Services.Models;

namespace PehliDukaan.Services {
    public class ShopService {

        public int SaveOrder(Order order) {
            using (var context = new PDContext()) {
                context.Orders.Add(order);
                return context.SaveChanges();
            }
        }
    }
}
