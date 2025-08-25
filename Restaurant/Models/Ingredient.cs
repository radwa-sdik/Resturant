using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [ValidateNever]
        public ICollection<ProductIngredient>? ProductIngredients { get; set; }
    }

}
