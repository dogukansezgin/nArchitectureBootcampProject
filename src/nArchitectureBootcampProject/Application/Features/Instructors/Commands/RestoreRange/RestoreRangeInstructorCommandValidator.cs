using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Instructors.Commands.Delete;
using FluentValidation;

namespace Application.Features.Instructors.Commands.RestoreRange;

internal class RestoreRangeInstructorCommandValidator : AbstractValidator<RestoreRangeInstructorCommand>
{
    public RestoreRangeInstructorCommandValidator()
    {
        RuleFor(c => c.Ids).NotEmpty();
    }
}
