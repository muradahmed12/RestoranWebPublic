using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestoranWeb.Models
{
    public class FoodType : SharedModel
    {
        [Required(ErrorMessage = "Name field is required.")]
        [StringLength(100)]
        [Display(Name = "FoodType Name")]
        [Remote("CategoryNameCheck", "RemoteValidations", AdditionalFields = "Id", ErrorMessage = "Name already exists")]
        public string Name { get; set; }

        //[Required(ErrorMessage = "The Description field is required.")]
        //[StringLength(500, MinimumLength = 5, ErrorMessage = "Valid length 5 to 500")]
        public string Description { get; set; }

      

        [NotMapped]
        public IFormFile Logo { get; set; }

        [StringLength(200)]
        public string LogoUrl { get; set; }

        //[InverseProperty("Category")]
        public List<FoodItem> CategoryWiseFood { get; set; }
        
      

        public CategoryType Type { get; set; }
    }

    public class FoodTypeViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public bool Status { get; set; }
        public string LogoUrl { get; set; }
        public int CategoryWiseFood { get; set; }


        public CategoryType Type { get; set; }
    }

    public enum CategoryType
    {
        Lunch = 0,
        Dinner = 10,
        BreakFast = 20
    }
   
}
