using PehliDukaan.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PehliDukaan.Database.DataService {
    public class ConfigurationsService {

        public static ConfigurationsService ClassObject {

            get {
                if (privateInMemoryObject == null) privateInMemoryObject = new ConfigurationsService();
                return privateInMemoryObject;
            }


        }
        private static ConfigurationsService privateInMemoryObject { get; set; }

        private ConfigurationsService() {


        }


        public Config GetConfig(string key) {

            using (var context = new PDContext()) {

                return context.Configurations.Find(key);
            }
        }
    }
}
