using Restaurant.Models;

namespace Restaurant.ViewModel
{
    public class OrderVM
    {
        private decimal _TotalAmount;
        public decimal TotalAmount { get => OrderItems.Sum(i => i.Price * i.Quantity); set => _TotalAmount = value; }
        public List<OrderItemVM> OrderItems { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
