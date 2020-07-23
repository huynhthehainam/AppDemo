using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AMG.App.DAL.Databases;

namespace AMG.App.API.Validations
{
    public class ExistedEmailValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var context = (DatabaseContext)validationContext.GetService(typeof(DatabaseContext));
            var email = Convert.ToString(value);
            if (!context.Users.Any(ww => ww.Email == email))
            {
                return new ValidationResult($"Email or password is invalid");
            }
            return ValidationResult.Success;
        }
    }
}