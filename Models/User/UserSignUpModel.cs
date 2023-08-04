using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace ALEXforums.Models.User
{
    public class UserSignUpModel
    {
        [FromForm(Name = "user-login")]  
        public String Login { get; set; } = null!;

        [FromForm(Name = "user-password")]
        public String Password { get; set; } = null!;

        [FromForm(Name = "user-repeat")]
        public String RepeatPassword { get; set; } = null!;

        [FromForm(Name = "user-avatar")]
        public IFormFile AvatarFile { get; set; } = null!;

    }
}
