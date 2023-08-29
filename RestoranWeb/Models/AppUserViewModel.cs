using RestoranWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace RestoranWeb.Models
{
    public class AppUserViewModel : SharedModel
    {
        public string Name { get; set; }
     
        public string Email { get; set; }

        public List<string> AppRole { get; set; }
    }
}
