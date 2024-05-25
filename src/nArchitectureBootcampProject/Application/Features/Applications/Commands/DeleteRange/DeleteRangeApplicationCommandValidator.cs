using FluentValidation;

namespace Application.Features.Applications.Commands.DeleteRange;

public class DeleteRangeApplicationCommandValidator : AbstractValidator<DeleteRangeApplicationCommand>
{
    public DeleteRangeApplicationCommandValidator()
    {
        RuleFor(c => c.Ids).NotEmpty();
    }
}
