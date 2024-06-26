﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Instructors.Commands.Delete;
using FluentValidation;

namespace Application.Features.Applicants.Commands.RestoreRange;

internal class RestoreRangeApplicantCommandValidator : AbstractValidator<RestoreRangeApplicantCommand>
{
    public RestoreRangeApplicantCommandValidator()
    {
        RuleFor(c => c.Ids).NotEmpty();
    }
}
