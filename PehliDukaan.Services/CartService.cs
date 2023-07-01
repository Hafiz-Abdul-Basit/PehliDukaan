using PehliDukaan.Database;
using PehliDukaan.Entities;
using PehliDukaan.Services.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace PehliDukaan.Services {
    public class CartService {

        private PDContext _context;

        public CartService() {
            _context = new PDContext();
        }

        public async Task<bool> AddOrUpdateProductsAsync(IEnumerable<ProductCartCookie> products, string userId) {
            // Get existing products from the DB
            var entities = await _context.CartProducts
                .Where(e => e.UserId == userId)
                .ToListAsync();

            // Remove existing cart items for the user
            _context.CartProducts.RemoveRange(entities);

            // Add new cart items
            var cartItems = products.Select(p => new CartProduct {
                ProductId = p.Id,
                Quantity = p.Quantity,
                UserId = userId
            });
            _context.CartProducts.AddRange(cartItems);

            // Save changes to the database
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<CartProduct>> GetCartItems(string userId) {
            return await _context.CartProducts
                .Where(cp => cp.UserId == userId)
                .ToListAsync();
        }

        public async Task<CartProduct> GetCartItem(string userId, int id) {
            return await _context.CartProducts
                .FirstOrDefaultAsync(cp => cp.UserId == userId && cp.Id == id);
        }

        public async Task<bool> DeleteCartProductAsync(string userId, int productId) {
            var cartProduct = await _context.CartProducts
                .FirstOrDefaultAsync(cp => cp.UserId == userId && cp.ProductId == productId);

            if (cartProduct == null) {
                return false;
            }

            _context.CartProducts.Remove(cartProduct);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task RemoveCartItemsAsync(string userId) {
            var cartItems = await _context.CartProducts
                .Where(c => c.UserId == userId)
                .ToListAsync();

            _context.CartProducts.RemoveRange(cartItems);

            await _context.SaveChangesAsync();
        }
    }
}
