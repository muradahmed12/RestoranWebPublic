using System.ComponentModel.DataAnnotations;

namespace RestoranWeb.Models
{
    public class OrderDetail : SharedModel
    {
        [Required]
        public string OrderId { get; set; }

        // Navigation property for the order to which this detail belongs
        public Order Order { get; set; }

        [Required]
        public string FoodItemId { get; set; }

        // Navigation property for the product in this detail
        public FoodItem FoodItem { get; set; }

        public int QuantityDemanded { get; set; }
        public int? QuantitySent { get; set; }

        // Additional relevant details
    }
}
