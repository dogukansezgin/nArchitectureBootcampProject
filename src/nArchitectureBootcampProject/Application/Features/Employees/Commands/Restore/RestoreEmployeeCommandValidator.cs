using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Instructors.Commands.Delete;
using FluentValidation;

namespace Application.Features.Employees.Commands.Restore;

public class RestoreEmployeeCommandValidator : AbstractValidator<RestoreEmployeeCommand>
{
    public RestoreEmployeeCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
