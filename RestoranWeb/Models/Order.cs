using RestoranWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace RestoranWeb.Models
{
    public class Order :SharedModel
    {
        [Required]
        public string UserId { get; set; }

        // Navigation property for the user who placed this order
        public AppUser User { get; set; }

        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "The DeliveryAddress field is required.")]
        [StringLength(500)]
        public string DeliveryAddress { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }

        public List<OrderDetail> Details { get; set; }
    }
}
