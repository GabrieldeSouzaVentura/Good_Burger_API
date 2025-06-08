using GoodBurger.Models;
using System.ComponentModel.DataAnnotations;

namespace GoodBurger.Validations;

public class BurgerValidation : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var order = (Order)validationContext.GetService(typeof(Order));
        int burger = (int)value;

        if (burger == null || order.Burger.BurgerId == null) return new ValidationResult("Invalid burger");

        if (order.Burger.BurgerId > 1 || order.Burger.BurgerId < 1) return new ValidationResult("Invalid burger");

        if (burger != order.Burger.BurgerId) return new ValidationResult("This burger is not on the Menu");

        return ValidationResult.Success;
    }
}
