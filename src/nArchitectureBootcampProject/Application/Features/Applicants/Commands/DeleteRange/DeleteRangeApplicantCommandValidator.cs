using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Instructors.Commands.Delete;
using FluentValidation;

namespace Application.Features.Applicants.Commands.DeleteRange;

public class DeleteRangeApplicantCommandValidator : AbstractValidator<DeleteRangeApplicantCommand>
{
    public DeleteRangeApplicantCommandValidator()
    {
        RuleFor(c => c.Ids).NotEmpty();
    }
}
