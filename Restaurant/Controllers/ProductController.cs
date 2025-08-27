using Microsoft.AspNetCore.Mvc;
using Restaurant.Models;
using Restaurant.Repository;

namespace Restaurant.Controllers
{
    public class ProductController : Controller
    {
        private readonly IRepository<Product> _productRepo;
        private readonly IRepository<Ingredient> _ingredientRepo;
        private readonly IRepository<Category> _categoryRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IRepository<Product> productRepo, IRepository<Ingredient> ingredientRepo, IRepository<Category> categoryRepo, IWebHostEnvironment webHostEnvironment)
        {
            _productRepo = productRepo;
            _ingredientRepo = ingredientRepo;
            _categoryRepo = categoryRepo;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> products = await _productRepo.GetAllAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            ViewBag.Ingredients = await _ingredientRepo.GetAllAsync();
            ViewBag.Categories = await _categoryRepo.GetAllAsync();

            if (id == 0) {
                ViewBag.Operation = "Add";
                return View();
            }
            else
            {
                ViewBag.Operation = "Edit";
                var product = await _productRepo.GetByIdAsync(id, new QueryOptions<Product>() { Includes = $"{nameof(Product.Category)},{nameof(Product.ProductIngredients)}.{nameof(ProductIngredient.Ingredient)}"});
                return View(product);
            }
        }

        [HttpPost]
        public  async Task<IActionResult> AddEdit(Product product, int[] ingredientIds)
        {
            ViewBag.Ingredients = await _ingredientRepo.GetAllAsync();
            ViewBag.Categories = await _categoryRepo.GetAllAsync();

            if (ModelState.IsValid) {
                if (product.ImageFile != null) {
                    string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "image");
                    string fileName = Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;
                    string filePath = Path.Combine(uploadFolder, fileName);
                    using(var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(fileStream);
                    }

                    product.ImageUrl = fileName;
                }                

                if (product.ProductId == 0)
                {                 
                    await _productRepo.AddAsync(product);
                    foreach (var ingredientId in ingredientIds)
                    {
                        product.ProductIngredients?.Add(new ProductIngredient { ProductId = product.ProductId, IngredientId = ingredientId });
                    }

                    return RedirectToAction("Index","Product");
                }
                else
                {
                    var existingProduct = await _productRepo.GetByIdAsync(product.ProductId, new QueryOptions<Product>() { Includes = $"{nameof(Product.ProductIngredients)}" });

                    if (existingProduct == null) {
                        ModelState.AddModelError("", "This Product Dosen't Exist");

                        return View(product);
                    }

                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.CategoryId = product.CategoryId;
                    existingProduct.ImageUrl = product.ImageUrl;
                    existingProduct.Stock = product.Stock;

                    existingProduct.ProductIngredients?.Clear();
                    foreach (var ingredientId in ingredientIds)
                    {
                        existingProduct.ProductIngredients?.Add(new ProductIngredient { ProductId = existingProduct.ProductId, IngredientId = ingredientId });
                    }

                    try
                    {
                        await _productRepo.UpdateAsync(existingProduct);
                    }catch(Exception ex)
                    {
                        ModelState.AddModelError("", ex.GetBaseException().Message);

                        return View(product);
                    }

                    return RedirectToAction("Index", "Product");
                }

            }

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {

            await _productRepo.RemoveAsync(id);
            return RedirectToAction("Index", "Product");
        }
    }
}
