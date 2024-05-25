using Application.Features.Instructors.Commands.Delete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ApplicationStates.Commands.DeleteRange;
public class DeleteRangeApplicationStateCommandValidator : AbstractValidator<DeleteRangeApplicationStateCommand>
{
    public DeleteRangeApplicationStateCommandValidator()
    {
        RuleFor(c => c.Ids).NotEmpty();
    }
}
