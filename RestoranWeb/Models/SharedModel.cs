using System.ComponentModel.DataAnnotations;

namespace RestoranWeb.Models
{
    public class SharedModel
    {
       
        public SharedModel()
        {
            Id = Path.GetRandomFileName().Replace(".", "");
            DbEntryTime = DateTime.UtcNow;
        }

        [ScaffoldColumn(false)]
        public string Id { get; set; }
        
        [ScaffoldColumn(false)]
        public DateTime DbEntryTime { get; set; }
    }
}
