using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Instructors.Commands.Delete;
using FluentValidation;

namespace Application.Features.BootcampStates.Commands.RestoreRange;

internal class RestoreRangeBootcampStateCommandValidator : AbstractValidator<RestoreRangeBootcampStateCommand>
{
    public RestoreRangeBootcampStateCommandValidator()
    {
        RuleFor(c => c.Ids).NotEmpty();
    }
}
