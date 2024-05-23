using FluentValidation;

namespace Application.Features.Applications.Commands.DeleteSelected;

public class DeletedSelectedApplicationCommandValidator : AbstractValidator<DeleteSelectedApplicationCommand>
{
    public DeletedSelectedApplicationCommandValidator()
    {
        RuleFor(a => a.Ids).NotEmpty();
    }
}
