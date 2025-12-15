using System.Runtime.CompilerServices;
using JobApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace JobApplication.Controllers
{
    public class ProfileController : Controller
    {
        private readonly JobContext _context;

        public ProfileController(JobContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            string useridstr = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(useridstr))
            {
                return RedirectToAction("Index", "Login");
            }
            int UserID = int.Parse(useridstr);
            var users = _context.Users.FirstOrDefault(u => u.UserID == UserID);
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }
        [HttpGet]
        public IActionResult Edit() 
        {
            string useridstr = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(useridstr))
            {
                return RedirectToAction("Index", "Login");
            }
            int UserID = int.Parse(useridstr);
            var users = _context.Users.FirstOrDefault(u=>u.UserID == UserID);
            if (users == null) 
            {
                return NotFound();
            }
            return View(users);
        }
        [HttpPost]
        public IActionResult Edit(UserModel model) 
        {
            string useridstr = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(useridstr)) 
            {
                return RedirectToAction("Index", "Login");
            }
            int userID = int.Parse(useridstr);
            var user = _context.Users.FirstOrDefault(u=>u.UserID == userID);
            if (user == null)
            {
                return NotFound();
            }
            user.Name = model.Name;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Skills = model.Skills;

            _context.SaveChanges();

            HttpContext.Session.SetString("Name", user.Name);
            TempData["message"] = "Update Successful";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult EmployerIndex() 
        {
            string useridstr = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(useridstr))
            {
                return RedirectToAction("Index","Login");
            }
            int UserID = int.Parse(useridstr);
            var Company_Profile = _context.Users.FirstOrDefault(u=> u.UserID == UserID);
            if (Company_Profile == null)
            {
                return NotFound();
            }
            return View(Company_Profile);
        }
        [HttpGet]
        public IActionResult EditEmployer()
        {
            string useridstr = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(useridstr))
            {
                RedirectToAction("Index", "Login");
            }
            int UserID =int.Parse(useridstr);
            var Company_Profile = _context.Users.FirstOrDefault(u=>u.UserID == UserID);
            if (Company_Profile == null)
            {
                return NotFound();
            }
            return View(Company_Profile);

        }
        [HttpPost]
        public IActionResult EditEmployer(UserModel model) 
        {
             string useridstr = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(useridstr))
            {
                return RedirectToAction("Index", "Login");
            }
            int UserID =int.Parse(useridstr);
            var Company_Profile = _context.Users.FirstOrDefault(u=>u.UserID == UserID);

            if (Company_Profile == null)
            {
                return NotFound();
            }

            Company_Profile.Name = model.Name;
            Company_Profile.Email = model.Email;
            Company_Profile.PhoneNumber = model.PhoneNumber;

            HttpContext.Session.SetString("Name", Company_Profile.Name);
            _context.SaveChanges();
            TempData["Message"] = "Update Sucessful";
            return RedirectToAction("EmployerIndex");
        }
    
    }
}
