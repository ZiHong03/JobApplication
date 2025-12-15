using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobApplication.Models;

namespace JobApplication.Controllers
{
    public class EmployerController : Controller
    {
        private readonly JobContext _context;

        public EmployerController(JobContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            var employerIdStr = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(employerIdStr))
            {
                return RedirectToAction("Index", "Login");
            }

            int employerId = int.Parse(employerIdStr);

            var jobs = _context.Jobs
                        .Where(j => j.EmployerID == employerId)
                        .Include(j => j.Applications)
                        .ToList();

            return View(jobs);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var employerIdStr = HttpContext.Session.GetString("UserID");
            var companyName = HttpContext.Session.GetString("Name"); //CompanyName 放在这里
            //HttpContext.Session.SetString("CompanyName",users.Name);  

            if (string.IsNullOrEmpty(employerIdStr))
            {
                return RedirectToAction("Index", "Login");
            }

            var job = new JobModel
            {
                CompanyName = companyName
            };

            return View(job);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(JobModel model)
        {
            var employerIdStr = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(employerIdStr))
            {
                return RedirectToAction("Index", "Login");
            }
            Console.WriteLine(ModelState.IsValid);

            if (ModelState.IsValid)
            {
                model.EmployerID = int.Parse(employerIdStr);
                model.CreatedAt = DateTime.Now;

                _context.Jobs.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Dashboard");
            }
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Key: {state.Key}, Error: {error.ErrorMessage}");
                    }
                }
                return View(model);
            }


            return View(model);
        }


        public IActionResult Applications(int jobId)
        {
            var apps = _context.Applications
                .Include(a => a.User)
                .Include(a => a.Job)
                .Where(a => a.JobID == jobId)
                .ToList();

            return View(apps);
        }

        // ==============================
        // Update Application Status
        // ==============================
        [HttpPost]
        public IActionResult UpdateStatus(int applicationId, ApplicationStatus status)
        {
            var application = _context.Applications.FirstOrDefault(a => a.ApplicationID == applicationId);

            if (application != null)
            {
                application.Status = status;
                _context.SaveChanges();
            }

            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public IActionResult DownloadCV(int applicationId)
        {
            string role = HttpContext.Session.GetString("UserRole");
            string userIdStr = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(userIdStr))
            {
                return RedirectToAction("Index", "Login");
            }

            int employerId = int.Parse(userIdStr);

            var application = _context.Applications
                .Include(a => a.Job)
                .FirstOrDefault(a =>
                    a.ApplicationID == applicationId &&
                    a.Job.EmployerID == employerId
                );

            if (application == null || string.IsNullOrEmpty(application.CVFilePath))
            {
                return NotFound();
            }

            var relativePath = application.CVFilePath.TrimStart('/');

            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                relativePath
            );

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"File not found: {filePath}");
            }

            return PhysicalFile(filePath, "application/pdf", Path.GetFileName(filePath));
        }

        [HttpGet]
        public IActionResult JobDetail(int jobid)
        {
            string useridstr = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(useridstr)) 
            {
                return RedirectToAction("Index","Login");
            }
            var job = _context.Jobs.FirstOrDefault(j => j.JobID == jobid);
            if (job == null)
            {
                return NotFound();
            }
            return View(job);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int jobid)
        {
            string useridstr = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(useridstr))
            {
                return RedirectToAction("Index", "Login");
            }

            var applications = _context.Applications.Where(a => a.JobID == jobid).ToList();
            _context.Applications.RemoveRange(applications);

            var job = _context.Jobs.FirstOrDefault(j => j.JobID == jobid);
            if (job == null)
            {
                return NotFound();
            }

            _context.Jobs.Remove(job);
            _context.SaveChanges();   

            TempData["Message"] = "Delete Successful";
            return RedirectToAction("Dashboard");
        }
    }
}
