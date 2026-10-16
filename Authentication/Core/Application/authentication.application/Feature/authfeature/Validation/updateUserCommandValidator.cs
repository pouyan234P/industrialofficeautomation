using authentication.application.Feature.authfeature.request.Commands;
using FluentValidation;

namespace authentication.application.Feature.authfeature.Validation
{
    public class updateUserCommandValidator : AbstractValidator<updateUserCommand>
    {
        public updateUserCommandValidator()
        {
            RuleFor(x => x.id)
                .GreaterThan(0).WithMessage("A valid user ID is required.");

            RuleFor(x => x.userDTO)
                .NotNull().WithMessage("User data is required.");

            When(x => x.userDTO != null, () =>
            {
                RuleFor(x => x.userDTO!.username)
                    .NotEmpty().WithMessage("Username is required.")
                    .MaximumLength(256).WithMessage("Username must not exceed 256 characters.");

                RuleFor(x => x.userDTO!.personID)
                    .GreaterThan(0).WithMessage("A valid PersonID is required.");

                RuleFor(x => x.userDTO!.password)
                    .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
                    .When(x => !string.IsNullOrEmpty(x.userDTO!.password));

                RuleFor(x => x.userDTO!.signitureImageId)
                    .GreaterThan(0).WithMessage("A valid signature image ID is required.")
                    .When(x => x.userDTO!.signitureImageId.HasValue);
            });
        }
    }
}
