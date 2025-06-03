using System.ComponentModel.DataAnnotations;

namespace contactAppMicroservice.Validation
{
    public class DateInThePast: ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null) return false;

            if (value is DateOnly date)
            {
                return date < DateOnly.FromDateTime(DateTime.Now);
            }

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return name + " must be in the past";
        }
    }
}
