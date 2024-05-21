using FluentValidation;

namespace Application.Features.Bootcamps.Commands.DeleteRange;

public class DeleteRangeBootcampCommandValidator : AbstractValidator<DeleteRangeBootcampCommand>
{
    public DeleteRangeBootcampCommandValidator()
    {
        RuleFor(c => c.Ids).NotEmpty();
        RuleFor(c => c.IsPermament).NotEmpty();
    }
}
