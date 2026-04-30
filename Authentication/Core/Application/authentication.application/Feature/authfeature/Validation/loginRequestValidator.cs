using authentication.application.Feature.authfeature.request.Queries;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.Feature.authfeature.Validation
{
    public class loginRequestValidator : AbstractValidator<loginRequest>
    {
        public loginRequestValidator()
        {
            RuleFor(x => x.myslogindto)
                .NotNull().WithMessage("Login data is required.");

            When(x => x.myslogindto != null, () =>
            {
                RuleFor(x => x.myslogindto!.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("A valid email address is required.");

                RuleFor(x => x.myslogindto!.Password)
                    .NotEmpty().WithMessage("Password is required.")
                    .MinimumLength(8).WithMessage("Password must be at least 8 characters.");
            });
        }
    
    }
}
