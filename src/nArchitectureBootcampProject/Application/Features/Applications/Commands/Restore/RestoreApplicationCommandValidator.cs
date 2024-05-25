using FluentValidation;

namespace Application.Features.Applications.Commands.Restore;

public class RestoreApplicationCommandValidator : AbstractValidator<RestoreApplicationCommand>
{
    public RestoreApplicationCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
