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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ingredient ingredient) {
            if (ModelState.IsValid) {
                await _ingrediantRepo.AddAsync(ingredient);
                return RedirectToAction("Index");
            }
          
            return View(ingredient);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id) {
            Ingredient? ingredient = await _ingrediantRepo.GetByIdAsync(id, new QueryOptions<Ingredient>());
            return View(ingredient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Ingredient ingredient)
        {
            if (ModelState.IsValid) { 
                await _ingrediantRepo.UpdateAsync(ingredient);
                return RedirectToAction("Index");
            }

            return View(ingredient);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _ingrediantRepo.RemoveAsync(id);
            return RedirectToAction("Index");
        }
    }
}
