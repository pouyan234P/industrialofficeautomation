using authentication.application.Feature.authfeature.request.Commands;
using FluentValidation;

namespace authentication.application.Feature.authfeature.Validation
{
    public class createRoleCommandValidator : AbstractValidator<createRoleCommand>
    {
        public createRoleCommandValidator()
        {
            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("Role name is required.")
                .MaximumLength(50).WithMessage("Role name must not exceed 50 characters.")
                .Matches(@"^\s*[A-Za-z][A-Za-z0-9 _-]*\s*$")
                    .WithMessage("Role name must start with a letter and contain only letters, digits, spaces, '-' or '_'.");
        }
    }
}
