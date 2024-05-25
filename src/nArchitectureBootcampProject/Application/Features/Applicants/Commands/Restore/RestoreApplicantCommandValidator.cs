using Application.Features.Instructors.Commands.Delete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Applicants.Commands.Restore;
public class RestoreApplicantCommandValidator : AbstractValidator<RestoreApplicantCommand>
{
    public RestoreApplicantCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
