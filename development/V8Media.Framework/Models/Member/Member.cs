using System.ComponentModel.DataAnnotations;

namespace V8Media.Framework.Models.Member
{
    public class MemberLoginModel
    {
        [Required, Display(Name = "Enter your user name")]
        public string Username { get; set; }

        [Required, Display(Name = "Password"), DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }

    }
}
    