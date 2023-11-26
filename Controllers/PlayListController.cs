
using Microsoft.AspNetCore.Mvc;
using BTL.Models ; 
namespace BTL.Controllers
{
    public class PlayListController : Controller
    {
        EntityModel entityModel ;  
        Audio[] Audios ; 
        public PlayListController(EntityModel entityModel , List<Audio> audios){
            this.entityModel = entityModel ; 
            Audios = audios.ToArray(); 
        }
      
        public IActionResult Play(){

            return RedirectToAction("main", new{ ID = 0 });
        }

        [HttpGet()]
        public IActionResult Main(int ID){
            if(Audios.Length != 0){
                 ViewData["max"] = Audios.Length ; 
                 // add to db 
                 DateControl Today = new DateControl(){Date = DateTime.Today} ; 
                AudioDate audioDate;
                 DateControl date = entityModel.Controls.ToList().Where(date => date.Date.Day == Today.Date.Day 
                                                                                && date.Date.Month == Today.Date.Month 
                                                                                && date.Date.Year == Today.Date.Year).FirstOrDefault();
                 if(date == null){
                     entityModel.Add(Today);
                     audioDate = new AudioDate(){AudioID = Audios[ID].ID , Date = Today}; 
                 }
                else{
                    audioDate = new AudioDate(){AudioID = Audios[ID].ID , Date = date};
                }
                 entityModel.Add(audioDate);
                 entityModel.SaveChanges();
                 return View(Audios[ID]);
            }
            return RedirectToAction("index","home");
        }

        [HttpGet()]
        public IActionResult FindTime(){
            return View();
        }

        [HttpPost()]
        public IActionResult FindTime(TimeModel timeModel){
            return View(timeModel);
        }
    }
}