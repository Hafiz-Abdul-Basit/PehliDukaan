namespace PehliDukaan.Services.Models {
    public class ProductCartCookie {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int CartId { get; set; }
    }
}
