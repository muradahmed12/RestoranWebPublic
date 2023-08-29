using RestoranWeb.Models;
using RestoranWeb.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RestoranWeb.Models
{
    public class AppRole : SharedModel
    {
        [Required(ErrorMessage = "The Name Feild is required")]
        [StringLength(50)]
        public string Name { get; set; }
        public List<AppUser> AppUsers { get; set; }
        
    }
}
