using System.Collections.Generic;

namespace PehliDukaan.Services.Models.Requests {
    public class UpdateItemsRequest {
        public IEnumerable<ProductCartCookie> Products { get; set; }
    }
}
