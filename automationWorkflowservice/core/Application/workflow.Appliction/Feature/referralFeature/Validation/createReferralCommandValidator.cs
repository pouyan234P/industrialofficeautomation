using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workflow.Appliction.Feature.referralFeature.request.Command;

namespace workflow.Appliction.Feature.referralFeature.Validation
{
    public class createReferralCommandValidator : AbstractValidator<createReferralCommand>
    {
        public createReferralCommandValidator()
        {
            RuleFor(x => x.setReferral)
                .NotNull().WithMessage("Referral data is required.");

            When(x => x.setReferral != null, () =>
            {
                RuleFor(x => x.setReferral!.LetterID)
                    .GreaterThan(0).WithMessage("A valid letter ID is required.");

                RuleFor(x => x.setReferral!.LetterSubject)
                    .NotEmpty().WithMessage("Letter subject is required.")
                    .MaximumLength(250).WithMessage("Letter subject must not exceed 250 characters.");

                RuleFor(x => x.setReferral!.LetterNo)
                    .NotEmpty().WithMessage("Letter number is required.")
                    .MaximumLength(50).WithMessage("Letter number must not exceed 50 characters.");

                RuleFor(x => x.setReferral!.SenderPositionID)
                    .GreaterThan(0).WithMessage("A valid sender position ID is required.");

                RuleFor(x => x.setReferral!.SenderName)
                    .NotEmpty().WithMessage("Sender name is required.")
                    .MaximumLength(100).WithMessage("Sender name must not exceed 100 characters.");

                RuleFor(x => x.setReferral!.SenderTitle)
                    .NotEmpty().WithMessage("Sender title is required.")
                    .MaximumLength(100).WithMessage("Sender title must not exceed 100 characters.");

                RuleFor(x => x.setReferral!.ReceiverPositionID)
                    .GreaterThan(0).WithMessage("A valid receiver position ID is required.");

                RuleFor(x => x.setReferral!.ReceiverName)
                    .NotEmpty().WithMessage("Receiver name is required.")
                    .MaximumLength(100).WithMessage("Receiver name must not exceed 100 characters.");

                RuleFor(x => x.setReferral!.ReceiverTitle)
                    .NotEmpty().WithMessage("Receiver title is required.")
                    .MaximumLength(100).WithMessage("Receiver title must not exceed 100 characters.");

                RuleFor(x => x.setReferral!.ActionType)
                    .IsInEnum().WithMessage("Action type must be a valid value.");

                RuleFor(x => x.setReferral!.Status)
                    .IsInEnum().WithMessage("Status must be a valid value.");

                RuleFor(x => x.setReferral!.Timestamp)
                    .NotEmpty().WithMessage("Timestamp is required.");

                
            });
        }
    }
}
