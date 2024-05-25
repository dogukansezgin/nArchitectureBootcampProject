using FluentValidation;

namespace Application.Features.Applications.Commands.RestoreRange;

public class RestoreRangeApplicationCommandValidator : AbstractValidator<RestoreRangeApplicationCommand>
{
    public RestoreRangeApplicationCommandValidator()
    {
        RuleFor(c => c.Ids).NotEmpty();
    }
}
