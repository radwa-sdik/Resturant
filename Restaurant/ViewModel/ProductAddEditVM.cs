using Restaurant.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Restaurant.ViewModel
{
    public class ProductAddEditVM
    {
        public int ProductId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }
        [Required]
        [Range(1, 1_000_000_000)]
        public decimal Price { get; set; } = 1m;
        [DisplayName("Quantity In Stock")]
        public int Stock { get; set; } = 0;
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public string? ImageUrl { get; set; } = "placeholder-image.png";
        [Required]
        public int CategoryId { get; set; }
        public int[] IngredientsIds { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
    }
}
