using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Instructors.Commands.Delete;
using FluentValidation;

namespace Application.Features.BootcampStates.Commands.Restore;

public class RestoreBootcampStateCommandValidator : AbstractValidator<RestoreBootcampStateCommand>
{
    public RestoreBootcampStateCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
