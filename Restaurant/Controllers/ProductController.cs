using Microsoft.AspNetCore.Mvc;
using Restaurant.Models;
using Restaurant.Repository;

namespace Restaurant.Controllers
{
    public class ProductController : Controller
    {
        private readonly IRepository<Product> _productRepo;
        public ProductController(IRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> products = await _productRepo.GetAllAsync();
            return View(products);
        }
    }
}
