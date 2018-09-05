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
            if (Startup.error != "")
            {
                ModelState.AddModelError("Error", Startup.error);
                Startup.error = "";
            }
            return View();
        }

        public IActionResult Code()
        {
            if (Startup.error != "")
            {
                ModelState.AddModelError("Error", Startup.error);
                Startup.error = "";
            }
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                Startup.error = "Please select an Image";
                return RedirectToAction("");
            }

            string name = Request.Form["name"].First();

            if (name == null || name == "select")
            {
                Startup.error = "Please enter your name";
                return RedirectToAction("");
            }

            string Event = Request.Form["Event"].First();

            if (Event == null || Event == "select")
            {
                Startup.error = "Please select an Event";
                return RedirectToAction("");
            }

            int period = name.IndexOf('.');
            string grade = name.Remove(period);
            string FullName = name.Remove(0, period+1);

            //Make sure not duplicate
            var path = Path.Combine(
                Startup.DataPath, 
                "Pictures", 
                grade, 
                FullName + "." + Event + "." + Startup.counts[0] + Path.GetExtension(file.FileName));


            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            Startup.counts[0] = (int.Parse(Startup.counts[0]) + 1).ToString();
            Startup.WriteCounts();

            Startup.error = "Upload";

            return RedirectToAction("");
        }

        //[HttpPost]
        //public IActionResult UploadCode(string code)
        //{
        //    if (code == null)
        //    {
        //        Startup.error = "Please enter a code";
        //        return RedirectToAction("Code");
        //    }

        //    string name = Request.Form["name"].First();

        //    if (name == null || name == "select")
        //    {
        //        Startup.error = "Please enter your name";
        //        return RedirectToAction("Code");
        //    }

        //    string[] lines = System.IO.File.ReadAllLines(Path.Combine(Startup.DataPath, "Settings", "SecretCodes.txt"));
        //    string found = "";
        //    for(int i = 0; i < lines.Count(); i++) {
        //        if (lines[i] == "") continue;
        //        if(lines[i].Equals(code, StringComparison.CurrentCultureIgnoreCase))
        //        {
        //            found = lines[i];
        //            lines[i] = "";
        //            break;
        //        }
        //    }

        //    if(found == "")
        //    {
        //        Startup.error = "Code not valid";
        //        return RedirectToAction("Code");
        //    }

        //    System.IO.File.WriteAllLines(Path.Combine(Startup.DataPath, "Settings", "SecretCodes.txt"), lines);

        //    string grade = name.Remove(2);
        //    string index = name.Remove(0, 2);
        //    string[] names = System.IO.File.ReadAllLines(Path.Combine(Startup.DataPath, "ClassOf20" + grade + ".txt"));
        //    string FullName = names[int.Parse(index)];

        //    //Make sure not duplicate
        //    var path = Path.Combine(
        //        Startup.DataPath,
        //        "Pictures",
        //        "ClassOf20" + grade,
        //        code.Replace(' ', '_') + ".txt");

        //    System.IO.File.WriteAllText(path, FullName);

        //    Startup.error = "Upload";

        //    return RedirectToAction("Code");
        //}
    }
}
