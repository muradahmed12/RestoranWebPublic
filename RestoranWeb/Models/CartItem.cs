using System.ComponentModel.DataAnnotations;

namespace RestoranWeb.Models
{
    public class CartItem:SharedModel
    {
        [Required]
        public string ShoppingCartId { get; set; }

        // Navigation property for the shopping cart to which this item belongs
        public ShoppingCart ShoppingCart { get; set; }

        [Required]
        public string FoodItemId { get; set; }

        // Navigation property for the product in this cart item
        public FoodItem FoodItem { get; set; }

        public int Quantity { get; set; }

        // Additional relevant details for the cart item
    }
}
