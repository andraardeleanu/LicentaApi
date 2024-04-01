using Api2.Requests;
using Core.Constants;
using FluentValidation;

namespace Api2.Validators
{
    public class CompanyValidator : CustomAbstractValidator<CompanyRequest>
    {
        public CompanyValidator()
        {           
            RuleFor(r => r.Cui).Must(x => !string.IsNullOrEmpty(x))               
                .WithMessage(ErrorMessages.MandatoryField);

            RuleFor(r => r.Cui).Must(cui => cui!.IsValidCui())
               .WithMessage(ErrorMessages.InvalidCui);
        }
    }
}
