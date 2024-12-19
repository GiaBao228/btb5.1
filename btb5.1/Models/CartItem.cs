using Microsoft.AspNetCore.Mvc;

namespace btb5._1.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total => Quantity * Price; // Tổng giá cho sản phẩm
    }
}
