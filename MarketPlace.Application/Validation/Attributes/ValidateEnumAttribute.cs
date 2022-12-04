using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Application.Validation.Attributes;

public class ValidateEnumAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value != null && value.GetType().IsEnum && !Enum.IsDefined(value.GetType(), value))
        {
            return new ValidationResult($"Provided value is not defined in enum {value.GetType().Name}");
        }

        return ValidationResult.Success;
    }
}