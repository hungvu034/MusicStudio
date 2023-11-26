using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace BTL.Models
{ 
    public class Audio
    {
        [KeyAttribute()]
        public string? ID{ get; set; }
        public string? Image { get; set; }
        public string? AudioSrc{get; set; }

        [Required(ErrorMessage = "{0} required")]
        public string Name {get; set ;}

        public List<AudioDate>? AudioDates{ get; set ;}
    }
}