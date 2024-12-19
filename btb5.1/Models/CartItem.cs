namespace btb5._1.Models
{
    public class CartItem
    {
        public int Id { get; set; } // Khóa chính
        public int ProductId { get; set; } // Mã sản phẩm
        public string ProductName { get; set; } = string.Empty; // Tên sản phẩm
        public decimal Price { get; set; } // Giá sản phẩm
        public int Quantity { get; set; } // Số lượng
        public string UserId { get; set; } = string.Empty; // ID người dùng (để phân biệt người mua)

        // Tùy chọn: Tính tổng tiền cho item
        public decimal Total => Price * Quantity;
    }
}
