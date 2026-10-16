using authentication.application.Feature.positionRepository.request.Commands;
using FluentValidation;

namespace authentication.application.Feature.positionRepository.Validation
{
    public class updatePositionCommandValidator : AbstractValidator<updatePositionCommand>
    {
        public updatePositionCommandValidator()
        {
            RuleFor(x => x.id)
                .GreaterThan(0).WithMessage("A valid position ID is required.");

            RuleFor(x => x.positionDTO)
                .NotNull().WithMessage("Position data is required.");

            When(x => x.positionDTO != null, () =>
            {
                RuleFor(x => x.positionDTO!.Title)
                    .NotEmpty().WithMessage("Position title is required.")
                    .MaximumLength(100).WithMessage("Position title must not exceed 100 characters.");

                RuleFor(x => x.positionDTO!.departmentId)
                    .GreaterThan(0).WithMessage("A valid department ID is required.");

                // Position.userId is a required column, so a position always needs a holder.
                RuleFor(x => x.positionDTO!.userId)
                    .GreaterThan(0).WithMessage("A position must have a holder (a valid user ID is required).");
            });
        }
    }
}
