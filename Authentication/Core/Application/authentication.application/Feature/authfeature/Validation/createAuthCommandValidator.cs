using authentication.application.Feature.authfeature.request.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.Feature.authfeature.Validation
{
    public class createAuthCommandValidator : AbstractValidator<createAuthCommand>
    {
        public createAuthCommandValidator()
        {
            RuleFor(x => x.registerdto)
                .NotNull().WithMessage("Registration data is required.");

            When(x => x.registerdto != null, () =>
            {
                RuleFor(x => x.registerdto!.Name)
                    .NotEmpty().WithMessage("First name is required.")
                    .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

                RuleFor(x => x.registerdto!.family)
                    .NotEmpty().WithMessage("Family name is required.")
                    .MaximumLength(50).WithMessage("Family name must not exceed 50 characters.");

                RuleFor(x => x.registerdto!.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("A valid email address is required.")
                    .MaximumLength(100).WithMessage("Email must not exceed 100 characters.");

                RuleFor(x => x.registerdto!.Password)
                    .NotEmpty().WithMessage("Password is required.")
                    .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
                    .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                    .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                    .Matches(@"[0-9]").WithMessage("Password must contain at least one digit.")
                    .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");

                RuleFor(x => x.registerdto!.phoneNumber)
                    .NotEmpty().WithMessage("Phone number is required.")
                    .Matches(@"^\+?[0-9]{7,15}$").WithMessage("Phone number must be between 7 and 15 digits and may start with '+'.");

                RuleFor(x => x.registerdto!.Country)
                    .NotEmpty().WithMessage("Country is required.")
                    .MaximumLength(60).WithMessage("Country must not exceed 60 characters.");

                RuleFor(x => x.registerdto!.Role)
                    .NotEmpty().WithMessage("Role is required.");

                RuleFor(x => x.registerdto!.personID)
                    .GreaterThan(0).WithMessage("A valid person ID is required.");
            });
        }
    }
}
