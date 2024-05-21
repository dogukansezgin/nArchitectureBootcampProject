using FluentValidation;

namespace Application.Features.Bootcamps.Commands.Restore;

public class RestoreBootcampCommandValidator : AbstractValidator<RestoreBootcampCommand>
{
    public RestoreBootcampCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
