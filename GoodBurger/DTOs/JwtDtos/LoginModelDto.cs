using System.ComponentModel.DataAnnotations;

namespace GoodBurger.DTOs.JwtDtos;

public class LoginModelDto
{
    [Required(ErrorMessage = "User name is required")]
    public string? UserName { get; set; }
    [Required(ErrorMessage = "Password is requires")]
    public string? Password { get; set; }
}
