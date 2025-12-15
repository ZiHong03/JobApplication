using Microsoft.EntityFrameworkCore;

namespace JobApplication.Models
{
    public class JobContext: DbContext
    {
        public JobContext(DbContextOptions<JobContext> options) : base(options)
        {
        }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<JobApplicationsModel> Applications { get; set; }
        public DbSet<JobModel>Jobs { get; set; }
    }
}
