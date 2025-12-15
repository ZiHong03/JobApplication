using System.ComponentModel.DataAnnotations;

namespace JobApplication.ViewModels
{
    public class UserViewModel
    {
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Password and Confirm Password do not match") ]
        public string ConfirmPassword { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string Skills { get; set; }
        [Required(ErrorMessage = "Please select user type")]
        public string Role { get; set; }

    }
}
