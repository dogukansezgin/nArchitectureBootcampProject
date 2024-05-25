using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Instructors.Commands.Delete;
using FluentValidation;

namespace Application.Features.Employees.Commands.RestoreRange;

internal class RestoreRangeEmployeeCommandValidator : AbstractValidator<RestoreRangeEmployeeCommand>
{
    public RestoreRangeEmployeeCommandValidator()
    {
        RuleFor(c => c.Ids).NotEmpty();
    }
}
