using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Email or Username")]
        public string EmailOrUsername { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }

        public string? ErrorMessage { get; set; }
    }
}