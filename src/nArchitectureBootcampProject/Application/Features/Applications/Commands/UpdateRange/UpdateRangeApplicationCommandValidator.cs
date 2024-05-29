using FluentValidation;

namespace Application.Features.Applications.Commands.UpdateRange;

public class UpdateRangeApplicationCommandValidator : AbstractValidator<UpdateRangeApplicationCommand>
{
    public UpdateRangeApplicationCommandValidator()
    {
        RuleFor(c => c.Applications).NotEmpty();
    }
}
