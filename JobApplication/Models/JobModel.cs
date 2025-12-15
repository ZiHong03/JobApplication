using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobApplication.Models
{
    public class JobModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JobID { get; set; }

        [Required]
        public int EmployerID { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string JobTitle { get; set; }

        [Required]
        public string Location { get; set; }

        public decimal Salary { get; set; }

        public string Remark { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<JobApplicationsModel> Applications { get; set; } = new List<JobApplicationsModel>();
    }
}
