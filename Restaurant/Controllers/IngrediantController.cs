using Microsoft.AspNetCore.Mvc;
using Restaurant.Models;
using Restaurant.Repository;

namespace Restaurant.Controllers
{
    public class IngrediantController : Controller
    {
        private readonly IRepository<Ingredient> _ingrediantRepo;
        public IngrediantController(IRepository<Ingredient> ingrediantRepo)
        {
            _ingrediantRepo = ingrediantRepo;
        }
        public async Task<IActionResult> Index()
        {
            var ingredients = await _ingrediantRepo.GetAllAsync();
            return View(ingredients);
        }

        public async Task<IActionResult> Details(int id)
        {
            var ingrediant = await _ingrediantRepo.GetByIdAsync(id, new QueryOptions<Ingredient>() {
                //Includes = "ProductIngredients.Product"
                Includes = $"{nameof(Ingredient.ProductIngredients)}.{nameof(ProductIngredient.Product)}" 
            }); 
            return View(ingrediant);
        }
    }
}
