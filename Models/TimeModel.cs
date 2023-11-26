using System.ComponentModel.DataAnnotations;
namespace BTL.Models{
    public class TimeModel{
        [RequiredAttribute()]
        public DateTime from { get; set ;}
        [Required()]
        public DateTime to{ get; set ;}
    }
}