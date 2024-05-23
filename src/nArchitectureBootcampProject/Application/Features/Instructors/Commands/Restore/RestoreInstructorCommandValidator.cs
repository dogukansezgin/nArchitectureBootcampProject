using Application.Features.Instructors.Commands.Delete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Instructors.Commands.Restore;
public class RestoreInstructorCommandValidator : AbstractValidator<RestoreInstructorCommand>
{
    public RestoreInstructorCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
