using authentication.application.Feature.departmentFeature.request.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.Feature.departmentFeature.Validation
{
    public class createDepartmentCommandValidator : AbstractValidator<createDepartmentCommand>
    {
        public createDepartmentCommandValidator()
        {
            RuleFor(x => x.departmentDTO)
                .NotNull().WithMessage("Department data is required.");

            When(x => x.departmentDTO != null, () =>
            {
                RuleFor(x => x.departmentDTO!.Name)
                    .NotEmpty().WithMessage("Department name is required.")
                    .MaximumLength(100).WithMessage("Department name must not exceed 100 characters.");

                RuleFor(x => x.departmentDTO!.ParentID)
                    .GreaterThanOrEqualTo(0).WithMessage("Parent ID must be zero (root) or a positive integer.");
            });
        }
    }
}
