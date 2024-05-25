using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Instructors.Commands.Delete;
using FluentValidation;

namespace Application.Features.ApplicationStates.Commands.Restore;

public class RestoreApplicationStateCommandValidator : AbstractValidator<RestoreApplicationStateCommand>
{
    public RestoreApplicationStateCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
