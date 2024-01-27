using Api2.Requests;
using FluentValidation;

namespace Api2.Validators
{
    public class CompanyValidator : CustomAbstractValidator<CompanyRequest>
    {
        public CompanyValidator()
        {           
            RuleFor(r => r.Cui).Must(x => !string.IsNullOrEmpty(x))               
                .WithMessage("Cuiul trebuie sa fie unic");            
        }
    }
}
