// Model pro registraci
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Uživatelské jméno je povinné")]
    [Display(Name = "Uživatelské jméno")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Heslo je povinné")]
    [StringLength(100, ErrorMessage = "Heslo musí mít alespoň 6 znaků", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Heslo")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Potvrdit heslo")]
    [Compare("Password", ErrorMessage = "Hesla se neshodují")]
    public string ConfirmPassword { get; set; }

    [Range(typeof(bool), "true", "true", ErrorMessage = "Musíte souhlasit s podmínkami použití")]
    public bool AcceptTerms { get; set; }
}