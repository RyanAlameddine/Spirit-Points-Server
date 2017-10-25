using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace SpiritPointsServer.Controllers
{
    public class HomeController : Controller
    {
        public enum MessageType { Success, Error, Info, Warning };

        public IActionResult Index()
        {
            if(Startup.error != "")
            {
                ModelState.AddModelError("Error", Startup.error);
                Startup.error = "";
            }
            return View();
        }

        //public IActionResult About()
        //{
        //    ViewData["Message"] = "Your application description page.";

        //    return View();
        //}

        //public IActionResult Contact()
        //{
        //    ViewData["Message"] = "Your contact page.";

        //    return View();
        //}

        public IActionResult Error()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, string FullName)
        {
            if (file == null || file.Length == 0)
            {
                Startup.error = "Please select an Image";
                return RedirectToAction("");
            }

            if(FullName == null || FullName == "")
            {
                Startup.error = "Please enter your name";
                return RedirectToAction("");
            }

            string grade = Request.Form["grade"].First();

            if(grade == null || grade == "select")
            {
                Startup.error = "Please select your grade";
                return RedirectToAction("");
            }

            //Make sure not duplicate
            var path = Path.Combine(
                Directory.GetCurrentDirectory(), 
                "Pictures", 
                "ClassOf20" + grade, 
                FullName.Replace(' ', '_') + "." + Startup.counts[0] + Path.GetExtension(file.FileName));


            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            Startup.counts[0] = (int.Parse(Startup.counts[0]) + 1).ToString();
            Startup.WriteCounts();

            return RedirectToAction("");
        }
    }
}
