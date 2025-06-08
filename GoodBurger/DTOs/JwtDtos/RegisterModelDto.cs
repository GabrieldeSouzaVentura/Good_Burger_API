using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace GoodBurger.DTOs.JwtDtos;

public class RegisterModelDto
{
    [Required(ErrorMessage = "User name is required")]
    public string? UserName { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Passwoprd is required")]
    public string Password { get; set; }
}
