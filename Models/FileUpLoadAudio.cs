using System.ComponentModel.DataAnnotations;
namespace BTL.Models
{
    public class FileUpLoadAudio
    {
        public FileUpLoadAudio(){
            Audio = new Audio();
        }
        public Audio? Audio{get ; set; }

        [RequiredAttribute(ErrorMessage = "FileImage Required")]
        public IFormFile ImageFileUpLoad{get; set; }

        [RequiredAttribute(ErrorMessage = "FileAudio Required")]
        public IFormFile AudioFileUpLoad{get; set; }
    }
}