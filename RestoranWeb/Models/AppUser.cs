using RestoranWeb.Models;
using RestoranWeb.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestoranWeb.Models
{
    public class AppUser : SharedModel
    {
        [Required(ErrorMessage = "Name field is required")]
        [StringLength(100, MinimumLength = 4)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Enter Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Enter  Valid email")]
        public string Email { get; set; }
        [NotMapped]

        public bool? EmailConfirmed { get; set; }

        [Required(ErrorMessage = "Enter PhoneNumber")]
        public string PhoneNumber { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Please Enter Password")]
        public string Password { get; set; }
     
        [Compare("Password", ErrorMessage = "Passwords not matched")]
        [NotMapped]
        public string ConfirmPassword { get; set; }

    
        public string HashPassword { get; set; }

        public List<AppRole> AppRoles { get; set; }
      
      


    }
  
}
