using System.ComponentModel.DataAnnotations;
using AMG.App.API.Validations;

namespace AMG.App.API.Models.Commands
{
    public class LoginCommand
    {
        [Required]
        [EmailAddress]
        [ExistedEmailValidation]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}