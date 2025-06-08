using GoodBurger.Models;
using System.ComponentModel.DataAnnotations;

namespace GoodBurger.Validations;

public class ExtraValidation : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var order = (Order)validationContext.GetService(typeof(Order));
        int extra = (int)value;

        if (order.Extras.Any(e => e.ExtraId == null)) return new ValidationResult("Invalid amount");

        if (order.Extras.Any(e => e.ExtraId !<= 2)) return new ValidationResult("Quantity of extras not allowed");

        if (order.Extras.Any(e => e.ExtraId != extra)) return new ValidationResult("The selected extra is not available on the menu");

        return ValidationResult.Success;
    }
}
