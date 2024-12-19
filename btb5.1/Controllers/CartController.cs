using btb5._1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;

namespace btb5._1.Controllers
{
    public class CartController : Controller
    {
        private const string CartSessionKey = "Cart";
        private readonly ApplicationDbContext _context;

        // Constructor để inject ApplicationDbContext
        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy giỏ hàng từ session
        private List<CartItem> GetCartItems()
        {
            var cartJson = HttpContext.Session.GetString(CartSessionKey);
            if (!string.IsNullOrEmpty(cartJson))
            {
                return JsonSerializer.Deserialize<List<CartItem>>(cartJson);
            }
            return new List<CartItem>();
        }

        // Lưu giỏ hàng vào session
        private void SaveCartItems(List<CartItem> cartItems)
        {
            var cartJson = JsonSerializer.Serialize(cartItems);
            HttpContext.Session.SetString(CartSessionKey, cartJson);
        }

        // Hiển thị giỏ hàng
        public IActionResult Index()
        {
            var cartItems = GetCartItems();
            return View(cartItems);
        }

        // Thêm sản phẩm vào giỏ hàng
        public IActionResult AddToCart(int productId)
        {
            // Lấy thông tin sản phẩm từ database
            var product = _context.Products.SingleOrDefault(p => p.Id == productId);
            if (product == null)
            {
                return NotFound(); // Nếu không tìm thấy sản phẩm
            }

            var cartItems = GetCartItems();
            var existingItem = cartItems.FirstOrDefault(c => c.ProductId == productId);

            if (existingItem != null)
            {
                // Tăng số lượng nếu sản phẩm đã có trong giỏ hàng
                existingItem.Quantity++;
            }
            else
            {
                // Thêm sản phẩm mới vào giỏ hàng
                cartItems.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.SellPrice,
                    Quantity = 1
                });
            }

            SaveCartItems(cartItems); // Lưu lại giỏ hàng vào session
            return RedirectToAction("Index");
        }

        // Xóa sản phẩm khỏi giỏ hàng
        public IActionResult RemoveFromCart(int productId)
        {
            var cartItems = GetCartItems();
            var itemToRemove = cartItems.FirstOrDefault(c => c.ProductId == productId);

            if (itemToRemove != null)
            {
                cartItems.Remove(itemToRemove);
                SaveCartItems(cartItems);
            }

            return RedirectToAction("Index");
        }

        // Cập nhật số lượng sản phẩm trong giỏ hàng
        [HttpPost]
        [HttpPost]
public JsonResult UpdateCart(int productId, int quantity)
{
    var cartItems = GetCartItems();
    var itemToUpdate = cartItems.FirstOrDefault(c => c.ProductId == productId);

    if (itemToUpdate != null && quantity > 0)
    {
        itemToUpdate.Quantity = quantity;
        SaveCartItems(cartItems);

        // Tính toán lại tổng cộng
        var itemTotal = itemToUpdate.Quantity * itemToUpdate.Price;
        var cartTotal = cartItems.Sum(c => c.Quantity * c.Price);

        return Json(new
        {
            success = true,
            itemTotal = itemTotal.ToString("C"),
            cartTotal = cartTotal.ToString("C")
        });
    }

    return Json(new { success = false });
}

    }
}
