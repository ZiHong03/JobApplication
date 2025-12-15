using JobApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;

namespace JobApplication.Controllers
{
    public class JobSeekerController : Controller
    {
        private readonly JobContext _context;

        public JobSeekerController (JobContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard(string search)
        {
            string UserIDString = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(UserIDString))
            {
                return RedirectToAction("Index", "Login");
            }
            int userid = int.Parse(UserIDString);

            var jobs = _context.Jobs.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                jobs = _context.Jobs.Where(j => j.JobTitle.Contains(search) ||
                       j.CompanyName.Contains(search) ||
                       j.Location.Contains(search));
            };
            var joblist = jobs.ToList();

            var appliedJobID = _context.Applications.Where(a=>a.UserID==userid).Select(a=>a.JobID).ToList();

            ViewBag.AppliedJob = appliedJobID;
            return View(joblist);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Apply(int jobId, IFormFile cvFile)
        {
            string userIDString = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(userIDString))
            {
                return RedirectToAction("Index", "Login");
            }


            if (cvFile == null || cvFile.Length == 0)
            {
                TempData["Message"] = "Please upload your CV";
                return RedirectToAction("Dashboard");
            }

            int userId = int.Parse(userIDString);

            string fileName = Guid.NewGuid() + Path.GetExtension(cvFile.FileName);
            string uploadPath = Path.Combine(Directory.GetCurrentDirectory(),
                                             "wwwroot/uploads/cv",
                                             fileName);

           
            using (var stream = new FileStream(uploadPath, FileMode.Create))
            {
                cvFile.CopyTo(stream);
            }

            var application = new JobApplicationsModel
            {
                UserID = userId,
                JobID = jobId,
                CVFilePath = "/uploads/cv/" + fileName,
                ApplyTime = DateTime.Now,
                Status = ApplicationStatus.Pending
            };

            _context.Applications.Add(application);
            _context.SaveChanges();

            TempData["Message"] = "Application submitted successfully!";
            return RedirectToAction("Dashboard");
        }


        public IActionResult Details(int jobID)
        {
            string userIDString = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(userIDString))
            {
                return RedirectToAction("Index", "Login");
            }
            //var job = _context.Jobs.Where(j=>j.JobID== jobID).ToList(); No suggest to use To.List()
            var job = _context.Jobs.FirstOrDefault(j => j.JobID== jobID);
            
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        [HttpGet]
        public IActionResult ViewApplication()
        {
            string userIDString = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(userIDString))
            {
                return RedirectToAction("Index", "Login");
            }

            int UserID = int.Parse(userIDString);

            var applicationRecord = _context.Applications.Include(a => a.Job).Where(a => a.UserID == UserID)
                .OrderByDescending(a => a.ApplyTime)
                .ToList();


            return View(applicationRecord);
        }




    }
}

