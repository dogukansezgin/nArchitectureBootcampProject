﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Instructors.Commands.Delete;
using FluentValidation;

namespace Application.Features.Instructors.Commands.DeleteRange;

public class DeleteRangeInstructorCommandValidator : AbstractValidator<DeleteRangeInstructorCommand>
{
    public DeleteRangeInstructorCommandValidator()
    {
        RuleFor(c => c.Ids).NotEmpty();
    }
}
