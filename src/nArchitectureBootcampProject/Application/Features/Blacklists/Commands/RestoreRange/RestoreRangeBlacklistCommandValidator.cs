using FluentValidation;

namespace Application.Features.Blacklists.Commands.RestoreRange;

public class RestoreRangeBlacklistCommandValidator : AbstractValidator<RestoreRangeBlacklistCommand>
{
    public RestoreRangeBlacklistCommandValidator()
    {
        RuleFor(c => c.Ids).NotEmpty();
    }
}
