using FluentValidation;
using FluentValidation.Results;
using Newtonsoft.Json;

namespace Api2.Validators
{
    public abstract class CustomAbstractValidator<T> : AbstractValidator<T>
    {
        public override ValidationResult Validate(ValidationContext<T> context)
        {
            var validationResult = base.Validate(context);

            if (!validationResult.IsValid)
            {
                var json = JsonConvert.SerializeObject(validationResult.Errors);
                throw new ValidationException(json);
            }

            return validationResult;
        }
    }
}
