using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workflow.Appliction.Feature.referralFeature.request.Command;

namespace workflow.Appliction.Feature.referralFeature.Validation
{
    public class updateReferralCommandValidator : AbstractValidator<updateReferralCommand>
    {
        public updateReferralCommandValidator()
        {
            RuleFor(x => x.referral)
                .NotNull().WithMessage("Referral data is required.");

            When(x => x.referral != null, () =>
            {
                RuleFor(x => x.referral.id)
                    .NotEmpty().WithMessage("A valid referral ID is required.");

                RuleFor(x => x.referral.LetterID)
                    .GreaterThan(0).WithMessage("A valid letter ID is required.");

                RuleFor(x => x.referral.LetterSubject)
                    .NotEmpty().WithMessage("Letter subject is required.")
                    .MaximumLength(250).WithMessage("Letter subject must not exceed 250 characters.");

                RuleFor(x => x.referral.LetterNo)
                    .NotEmpty().WithMessage("Letter number is required.")
                    .MaximumLength(50).WithMessage("Letter number must not exceed 50 characters.");

                RuleFor(x => x.referral.SenderPositionID)
                    .GreaterThan(0).WithMessage("A valid sender position ID is required.");

                RuleFor(x => x.referral.SenderName)
                    .NotEmpty().WithMessage("Sender name is required.")
                    .MaximumLength(100).WithMessage("Sender name must not exceed 100 characters.");

                RuleFor(x => x.referral.ReceiverPositionID)
                    .GreaterThan(0).WithMessage("A valid receiver position ID is required.");

                RuleFor(x => x.referral.ReceiverName)
                    .NotEmpty().WithMessage("Receiver name is required.")
                    .MaximumLength(100).WithMessage("Receiver name must not exceed 100 characters.");

                RuleFor(x => x.referral.ActionType)
                    .IsInEnum().WithMessage("Action type must be a valid value.");

                RuleFor(x => x.referral.Status)
                    .IsInEnum().WithMessage("Status must be a valid value.");
            });
        }
    }
}
