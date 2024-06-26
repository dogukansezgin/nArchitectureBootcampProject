﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Instructors.Commands.Delete;
using FluentValidation;

namespace Application.Features.ApplicationStates.Commands.RestoreRange;

internal class RestoreRangeApplicationStateCommandValidator : AbstractValidator<RestoreRangeApplicationStateCommand>
{
    public RestoreRangeApplicationStateCommandValidator()
    {
        RuleFor(c => c.Ids).NotEmpty();
    }
}
