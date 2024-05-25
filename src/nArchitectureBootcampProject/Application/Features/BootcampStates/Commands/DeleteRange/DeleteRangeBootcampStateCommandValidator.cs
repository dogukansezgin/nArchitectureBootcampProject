using Application.Features.Instructors.Commands.Delete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.BootcampStates.Commands.DeleteRange;
public class DeleteRangeBootcampStateCommandValidator : AbstractValidator<DeleteRangeBootcampStateCommand>
{
    public DeleteRangeBootcampStateCommandValidator()
    {
        RuleFor(c => c.Ids).NotEmpty();
    }
}
