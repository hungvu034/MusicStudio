using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
namespace BTL.Models
{
    public class DateControl
    {
        [KeyAttribute]
        public string ID{ get; set ; }
        public DateTime Date { get; set; }
        public List<AudioDate> AudioDates{ get; set; }
    }
}