using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;



namespace Clinic_App_01.Validators
{
    public class DateInFutureAttribute : ValidationAttribute , IClientModelValidator

    {
        public override bool IsValid(object? value)
        {
            if (value is not DateTime enteredDate)
            {
                return false; // Invalid type
            }

            DateTime now = DateTime.Now;
            return enteredDate >= now; // Ensures the date is in the future or present
        }

        // Adds client-side validation rule
        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-DateInFuture", ErrorMessage ?? "Date must be today or in the future.");
        }
    }
}