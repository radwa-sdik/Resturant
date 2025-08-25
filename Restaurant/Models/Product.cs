using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Models
{
    public class Product
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
        public int Stock { get; set; } = 0;
        [NotMapped]
        public IFormFile? ImageFile {  get; set; }
        public string? ImageUrl { get; set; } = "https://via.placeholder.com/150";
        [Required]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category? Category { get; set; }
        [ValidateNever]
        public ICollection<OrderItem>? OrderItems { get; set; }
        [ValidateNever]
        public ICollection<ProductIngredient>? ProductIngredients { get; set; }
    }
}
