using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobApplication.Models
{
    public enum ApplicationStatus
    {
        Pending,
        Accepted,
        Rejected
    }

    public class JobApplicationsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplicationID { get; set; }

        [Required]
        public int UserID { get; set; }
        public UserModel User { get; set; }

        [Required]
        public int JobID { get; set; }
        public JobModel Job { get; set; }

        [Required]
        public DateTime ApplyTime { get; set; } = DateTime.Now;

        [Required]
        public ApplicationStatus Status { get; set; }

        public string? CVFilePath { get; set; }
    }
}
