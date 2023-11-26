using System;
using System.ComponentModel.DataAnnotations;
namespace BTL.Models
{
    public class AudioDate
    {
        [KeyAttribute()]
        public string? ID{ get ; set; }

        public Audio? Audio{ get; set; }
        public string? AudioID{ get; set ;}
        public string DateID{ get; set; } 
        public DateControl Date{ get; set ;}
    }
}