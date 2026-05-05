using CorrespondenceCore.Application.Feature.LetterFeature.request.Command;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.Feature.LetterFeature.Validation
{
    public class createLetterCommandValidator : AbstractValidator<createLetterCommand>
    {
        public createLetterCommandValidator()
        {
            RuleFor(x => x.setLetterDTO)
                .NotNull().WithMessage("Letter data is required.");

            When(x => x.setLetterDTO != null, () =>
            {
                RuleFor(x => x.setLetterDTO!.Subject)
                    .NotEmpty().WithMessage("Subject is required.")
                    .MaximumLength(250).WithMessage("Subject must not exceed 250 characters.");

                RuleFor(x => x.setLetterDTO!.priority)
                    .IsInEnum().WithMessage("Priority must be a valid value (Normal, immediate, instantaneous).");

                RuleFor(x => x.setLetterDTO!.confidentiality)
                    .IsInEnum().WithMessage("Confidentiality must be a valid value (Normal, confidential, secret).");

                RuleFor(x => x.setLetterDTO!.type)
                    .IsInEnum().WithMessage("Type must be a valid value (Domestic, imported, exported).");

                RuleFor(x => x.setLetterDTO!.CreatorPositionID)
                    .GreaterThan(0).WithMessage("A valid creator position ID is required.");

                RuleFor(x => x.setLetterDTO!.BodyHTML)
                    .MaximumLength(50000).WithMessage("Body HTML must not exceed 50000 characters.")
                    .When(x => x.setLetterDTO!.BodyHTML != null);

                RuleFor(x => x.setLetterDTO!.ReplyToLetterID)
                    .GreaterThan(0).WithMessage("Reply-to letter ID must be a positive number.")
                    .When(x => x.setLetterDTO!.ReplyToLetterID.HasValue);
            });
        }
    }
}
