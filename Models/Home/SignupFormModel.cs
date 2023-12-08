using Microsoft.AspNetCore.Mvc;

namespace HW_4.Models.Home
{
    public class SignupFormModel
    {
        [FromForm(Name = "signup-fullname")]
        public string Fullname { get; set; } = null!;
        
        [FromForm(Name = "signup-email")]
        public string Email { get; set; } = null!;

        [FromForm(Name = "signup-phone")]
        public string Phone { get; set; } = null!; 
        
        [FromForm(Name = "signup-login")]
        public string Login { get; set; } = null!;

        [FromForm(Name = "signup-password")]
        public string Password { get; set; } = null!;

        [FromForm(Name = "signup-repeat")]
        public string Repeart { get; set; } = null!;

        [FromForm(Name = "signup-avatar")]
        public IFormFile Avatar { get; set; } = null!;
    }
}
