namespace PehliDukaan.Services.Models.Responses {
    public class CartProductResponse {
        public int Id { get; set; }
        public int ProductId { get; set; }


        public string Name { get; set; }

        public string UserId { get; set; }


        public string Description { get; set; }


        public decimal Price { get; set; }

        public int CategoryID { get; set; }

        public string ImageData { get; set; }

        public int Quantity { get; set; }
    }

}
