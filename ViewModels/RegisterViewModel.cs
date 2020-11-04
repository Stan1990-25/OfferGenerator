using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TMOffersClients
{
    public class RegisterViewModel
    {

        [Required]
        [MinLength(5)]
        [Display(Name = "Име")]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Невалиден Email адрес")]
        [Remote(action:"IsEmailInUse", controller:"Account")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Потвърди парола")]
        [Compare("Password", ErrorMessage = "Въведената и потвърдената парола не съвпадат")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Ниво на достъп:")]
        public string Role { get; set; }
    }

    public class UpdateUserViewModel : RegisterViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Текуща парола")]
        public string CurrentPassowrd { get; set; }
    }
}
