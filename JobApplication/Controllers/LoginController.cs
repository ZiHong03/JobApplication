using JobApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace JobApplication.Controllers
{
    public class LoginController : Controller
    {
        private readonly JobContext _context;

        public LoginController(JobContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string Email, string Password)
        {
            var users = _context.Users.FirstOrDefault(u=> u.Email ==Email&& u.Password==Password);

            if(users == null)
            {
                ViewBag.Error = "Invalid Email or Password";
                return View();
            }

            if (users.Role == "JobSeeker")
            {
                HttpContext.Session.SetString("UserEmail", users.Email);
                HttpContext.Session.SetString("UserRole", users.Role);
                HttpContext.Session.SetString("UserID", users.UserID.ToString());
                HttpContext.Session.SetString("Name", users.Name);

                ViewBag.Success = "Login Successfully";
                HttpContext.Session.SetString("UserID", users.UserID.ToString());
                return RedirectToAction("Dashboard", "JobSeeker"); //remember to add return
            }
            else if (users.Role == "Employer")
            {
                HttpContext.Session.SetString("UserEmail", users.Email);
                HttpContext.Session.SetString("Name",users.Name);
                HttpContext.Session.SetString("UserRole", users.Role);
                HttpContext.Session.SetString("UserID", users.UserID.ToString());

                ViewBag.Success = "Login Successfully";
                return RedirectToAction("Dashboard", "Employer");
            }

            ViewBag.Error = "Invalid Role";
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();   
            return RedirectToAction("Index", "Login"); 
        }
    }
}
