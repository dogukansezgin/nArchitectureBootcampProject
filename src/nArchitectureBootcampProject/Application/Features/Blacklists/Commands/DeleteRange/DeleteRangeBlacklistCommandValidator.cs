using FluentValidation;

namespace Application.Features.Blacklists.Commands.DeleteRange;

public class DeleteRangeBlacklistCommandValidator : AbstractValidator<DeleteRangeBlacklistCommand>
{
    public DeleteRangeBlacklistCommandValidator()
    {
        RuleFor(c => c.Ids).NotEmpty();
    }
}
