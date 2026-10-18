using authentication.application.Feature.authfeature.request.Commands;
using FluentValidation;

namespace authentication.application.Feature.authfeature.Validation
{
    public class setUserRolesCommandValidator : AbstractValidator<setUserRolesCommand>
    {
        public setUserRolesCommandValidator()
        {
            RuleFor(x => x.userId)
                .GreaterThan(0).WithMessage("A valid user ID is required.");

            RuleFor(x => x.roles)
                .NotNull().WithMessage("The list of roles is required (it may be empty).");

            RuleForEach(x => x.roles)
                .NotEmpty().WithMessage("Role names cannot be empty.")
                .MaximumLength(50).WithMessage("Role names must not exceed 50 characters.");
        }
    }
}
