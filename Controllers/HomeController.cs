using System.ComponentModel;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BTL.Models;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;

namespace BTL.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private EntityModel entityModel ; 
    private IWebHostEnvironment environment ; 

    private List<Audio> QuickLinks ; 
    public HomeController(ILogger<HomeController> logger , EntityModel entityModel , IWebHostEnvironment environment , List<Audio> Audios)
    {
        _logger = logger;
        this.entityModel = entityModel ; 
        this.environment = environment ; 
        QuickLinks = Audios ;    
    }

    public IActionResult Index()
    {
        Console.WriteLine(entityModel.Audios.Count());
        return View(entityModel.Audios.ToList());   
        
    }

    public IActionResult Privacy()
    {
        
        return View();
    }

    public IActionResult Add(){
        return View();
    }

    [HttpGet()]
    public IActionResult Detail(string ID){
        IEnumerable<Audio> Audios = entityModel.Audios.ToList() ; 
         Audio audio = Audios.Where(a => a.ID == ID).FirstOrDefault();
         return View(audio);
    }

    [HttpPost()]
    [ValidateAntiForgeryToken]   
    public IActionResult Add(FileUpLoadAudio fileUpLoad){

        if(ModelState.IsValid){  
            Console.WriteLine("valid");
            string WebRootPath = environment.WebRootPath ;   
            string AudioFileName = fileUpLoad.AudioFileUpLoad.FileName ; 
            string ImageFileName = fileUpLoad.ImageFileUpLoad.FileName ; 
            string AudioPath = Path.Combine(WebRootPath , "Media" , AudioFileName);
            string ImagePath = Path.Combine(WebRootPath, "images" , ImageFileName);
            FileStream audiostream = new FileStream(AudioPath , FileMode.Create) ; 
            fileUpLoad.AudioFileUpLoad.CopyTo(audiostream);  
            fileUpLoad.Audio.AudioSrc = fileUpLoad.AudioFileUpLoad.FileName ; 
            audiostream.Close(); 
            FileStream imagestream = new FileStream(ImagePath , FileMode.Create);  
            fileUpLoad.ImageFileUpLoad.CopyTo(imagestream); 
            fileUpLoad.Audio.Image = fileUpLoad.ImageFileUpLoad.FileName ; 
            imagestream.Close();  
            // add Audio 
            entityModel.Add(fileUpLoad.Audio);
            entityModel.SaveChanges(); 
            return RedirectToAction("index");
        }
        return View(fileUpLoad);
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Remove(string ID){
        var Audios = entityModel.Audios.ToList();
        var audio = Audios.Where(A => A.ID == ID).FirstOrDefault();
        entityModel.Remove(audio);
        entityModel.SaveChanges();
        audio = QuickLinks.Find(Audio => Audio.ID == ID);
        QuickLinks.Remove(audio);
        return RedirectToAction("index");
    }
    public class AudioCompare : IEqualityComparer<Audio>
    {
        public bool Equals(Audio? x, Audio? y)
        {
        
            return x.ID == y.ID; 
        }

        public int GetHashCode([DisallowNull] Audio obj)
        {
            return obj.GetHashCode();
        }
    }
    public IActionResult AddPlaylist(string ID){
        var Audios = entityModel.Audios.ToList();
        var audio = Audios.Where(A => A.ID == ID).First();
        if(!QuickLinks.Contains(audio , new AudioCompare())){
            QuickLinks.Add(audio);
        }
        return RedirectToAction("index"); 
    }
    public IActionResult Undo(){;
        if(QuickLinks.Count() != 0){
          QuickLinks.Remove(QuickLinks.Last());
        }
        return RedirectToAction("index");
    }

    public IActionResult Play(){
        return View();
    }

    public IActionResult Edit(string id){
        var FoundedAudio = ( from audios in entityModel.Audios
                            where audios.ID == id 
                            select audios ).FirstOrDefault();
        if(FoundedAudio != null){
            return View(new FileUpLoadAudio(){ Audio = FoundedAudio });
        }                      
        return Content("Not founded id"); 
    }
    [HttpPost()]
    public IActionResult Edit(FileUpLoadAudio fileUpLoadAudio){
        if(ModelState.IsValid){
            var CurrentID = fileUpLoadAudio.Audio.ID ;
            var CurrentAudio = ( from audios in entityModel.Audios
                            where audios.ID == CurrentID 
                            select audios ).FirstOrDefault();
            QuickLinks.RemoveAll(A => A.ID == CurrentID);
            entityModel.Remove(CurrentAudio);
            // delete file
            string ImagePath = Path.Combine(environment.WebRootPath , "images" , CurrentAudio.Image);
            string AudioPath = Path.Combine(environment.WebRootPath  , "Media" , CurrentAudio.AudioSrc);
    
            System.IO.File.Delete(ImagePath);
            System.IO.File.Delete(AudioPath);
            // update file
            fileUpLoadAudio.Audio.Image = fileUpLoadAudio.ImageFileUpLoad.FileName ; 
            fileUpLoadAudio.Audio.AudioSrc = fileUpLoadAudio.AudioFileUpLoad.FileName ; 
            ImagePath = Path.Combine(environment.WebRootPath , "images" , fileUpLoadAudio.ImageFileUpLoad.FileName);
            AudioPath = Path.Combine(environment.WebRootPath , "Media" , fileUpLoadAudio.Audio.AudioSrc);
            Stream fileStream = new FileStream(ImagePath , FileMode.Create);
            fileUpLoadAudio.ImageFileUpLoad.CopyTo(fileStream);
            fileStream.Close();
            fileStream = new FileStream(AudioPath , FileMode.Create);
            fileUpLoadAudio.AudioFileUpLoad.CopyTo(fileStream);
            fileStream.Close();
            // add to database
            entityModel.Add(fileUpLoadAudio.Audio);
            entityModel.SaveChanges();
            return RedirectToAction("index", "home"); 
        }
        return View(fileUpLoadAudio);
    }

    public IActionResult Move(string returnUrl){
        return LocalRedirect(returnUrl);
    }
}

