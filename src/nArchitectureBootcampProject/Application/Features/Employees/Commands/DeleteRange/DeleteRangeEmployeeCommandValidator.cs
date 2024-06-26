﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Instructors.Commands.Delete;
using FluentValidation;

namespace Application.Features.Employees.Commands.DeleteRange;

public class DeleteRangeEmployeeCommandValidator : AbstractValidator<DeleteRangeEmployeeCommand>
{
    public DeleteRangeEmployeeCommandValidator()
    {
        RuleFor(c => c.Ids).NotEmpty();
    }
}
