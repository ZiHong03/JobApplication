using JobApplication.Models;
using Microsoft.AspNetCore.Mvc;
using JobApplication.ViewModels;

namespace JobApplication.Controllers
{
    public class RegisterController : Controller
    {
        public readonly JobContext _context;

        public RegisterController (JobContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index (UserViewModel model)
        {
            if (!ModelState.IsValid) 
            {
                return View(model);
            }

            var ExistUsers = _context.Users.FirstOrDefault(u=> u.Email == model.Email );
            if (ExistUsers != null)
            {
                ViewBag.Error = "User exist";
                return View(model);
            }

            var users = new UserModel
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                PhoneNumber = model.PhoneNumber,
                Skills = model.Skills,
                Role = model.Role
            };
            _context.Users.Add(users); //Users 是Table name
            _context.SaveChanges();



            ViewBag.Success = "Register successful!";
            return RedirectToAction("Index","Login");
        }
        
    }
}
