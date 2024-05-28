using FluentValidation;

namespace Application.Features.Blacklists.Commands.Restore;

public class RestoreBlacklistCommandValidator : AbstractValidator<RestoreBlacklistCommand>
{
    public RestoreBlacklistCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
