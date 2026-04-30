using CorrespondenceCore.Application.Feature.LetterFeature.request.Command;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.Feature.LetterFeature.Validation
{
    public class updateLetterCommandValidator : AbstractValidator<updateLetterCommand>
    {
        public updateLetterCommandValidator()
        {
            RuleFor(x => x.letterDTO)
                .NotNull().WithMessage("Letter data is required.");

            When(x => x.letterDTO != null, () =>
            {
                RuleFor(x => x.letterDTO.Id)
                    .GreaterThan(0).WithMessage("A valid letter ID is required.");

                RuleFor(x => x.letterDTO.Subject)
                    .NotEmpty().WithMessage("Subject is required.")
                    .MaximumLength(250).WithMessage("Subject must not exceed 250 characters.");

                RuleFor(x => x.letterDTO.SentDate)
                    .NotEmpty().WithMessage("Sent date is required.");

                RuleFor(x => x.letterDTO.priority)
                    .IsInEnum().WithMessage("Priority must be a valid value (Normal, immediate, instantaneous).");

                RuleFor(x => x.letterDTO.confidentiality)
                    .IsInEnum().WithMessage("Confidentiality must be a valid value (Normal, confidential, secret).");

                RuleFor(x => x.letterDTO.type)
                    .IsInEnum().WithMessage("Type must be a valid value (Domestic, imported, exported).");

                RuleFor(x => x.letterDTO.CreatorPositionID)
                    .GreaterThan(0).WithMessage("A valid creator position ID is required.");
            });
        }
    }
}
