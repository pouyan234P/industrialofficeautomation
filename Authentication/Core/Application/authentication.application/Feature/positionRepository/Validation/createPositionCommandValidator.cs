using authentication.application.Feature.positionRepository.request.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.Feature.positionRepository.Validation
{
    public class createPositionCommandValidator : AbstractValidator<createPositionCommand>
    {
        public createPositionCommandValidator()
        {
            RuleFor(x => x.positionDTO)
                .NotNull().WithMessage("Position data is required.");

            When(x => x.positionDTO != null, () =>
            {
                RuleFor(x => x.positionDTO!.Title)
                    .NotEmpty().WithMessage("Position title is required.")
                    .MaximumLength(100).WithMessage("Position title must not exceed 100 characters.");

                RuleFor(x => x.positionDTO!.departmentId)
                    .GreaterThan(0).WithMessage("A valid department ID is required.");

                RuleFor(x => x.positionDTO!.userId)
                    .GreaterThan(0).WithMessage("A valid user ID is required.");
            });
        }
    }
}
