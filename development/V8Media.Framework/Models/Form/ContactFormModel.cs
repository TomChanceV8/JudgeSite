using System.ComponentModel.DataAnnotations;

namespace V8Media.Framework.Models.Form
{
    public class ContactFormModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
    }
}