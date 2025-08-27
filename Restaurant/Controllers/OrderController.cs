using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Data;
using Restaurant.Models;
using Restaurant.Repository;
using Restaurant.ViewModel;

namespace Restaurant.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IRepository<Order> _ordersRepo;
        private readonly IRepository<Product> _productsRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(ApplicationDbContext dbContext, IRepository<Order> ordersRepo, IRepository<Product> productRepo, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _ordersRepo = ordersRepo;
            _productsRepo = productRepo;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = HttpContext.Session.Get<OrderVM>("OrderViewModel") ?? new OrderVM() {
                OrderItems = new List<OrderItemVM>(),
                Products = await _productsRepo.GetAllAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(int productId, int quantity)
        {
            var product = await _productsRepo.GetByIdAsync(productId, new QueryOptions<Product>());
            if (product == null) return NotFound();

            var model = HttpContext.Session.Get<OrderVM>("OrderViewModel") ?? new OrderVM()
            {
                OrderItems = new List<OrderItemVM>(),
                Products = await _productsRepo.GetAllAsync()
            };

            //check if the product is already in the order
            var existingItem = model.OrderItems.FirstOrDefault(x => x.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                model.OrderItems.Add(new OrderItemVM()
                {
                    ProductId = productId,
                    Quantity = quantity,
                    Price = product.Price,
                    ProductName = product.Name
                });              
            }

            HttpContext.Session.Set("OrderViewModel", model);

            return RedirectToAction("Create", model);
        }

        [HttpGet]
        public async Task<IActionResult> Cart()
        {
            var model = HttpContext.Session.Get<OrderVM>("OrderViewModel");

            //if (model == null)
            //    return RedirectToAction("Create");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            var model = HttpContext.Session.Get<OrderVM>("OrderViewModel");

            if (model == null || model.OrderItems.Count == 0)
                return RedirectToAction("Create");

            Order order = new Order()
            {
                OrderDate = DateTime.Now,
                TotalPrice = model.TotalAmount,
                UserID =  _userManager.GetUserId(User)
            };

            foreach (var item in model.OrderItems) {
                order.OrderItems.Add(new OrderItem() { 
                    ProductID = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }

            await _ordersRepo.AddAsync(order);

            HttpContext.Session.Remove("OrderViewModel");

            return RedirectToAction("ViewOrders");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(int productId)
        {
            var product = await _productsRepo.GetByIdAsync(productId, new QueryOptions<Product>());
            if (product == null) return NotFound();

            var model = HttpContext.Session.Get<OrderVM>("OrderViewModel");

            var item = model.OrderItems.FirstOrDefault(x => x.ProductId == productId);
            model.OrderItems.Remove(item);

            HttpContext.Session.Set("OrderViewModel", model);

            return RedirectToAction("Cart");
        }

        [HttpGet]
        public async Task<IActionResult> ViewOrders()
        {
            var userId = _userManager?.GetUserId(User);            
            var userOrder = await _ordersRepo.GetAllAsync(userId, "UserID", new QueryOptions<Order>() { Includes = "OrderItems.Product"});

            return View(userOrder);
        }
    }
}
