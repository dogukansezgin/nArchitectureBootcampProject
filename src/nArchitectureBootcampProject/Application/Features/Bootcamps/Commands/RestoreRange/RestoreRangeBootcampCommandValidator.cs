using FluentValidation;

namespace Application.Features.Bootcamps.Commands.RestoreRange;

public class RestoreRangeBootcampCommandValidator : AbstractValidator<RestoreRangeBootcampCommand>
{
    public RestoreRangeBootcampCommandValidator()
    {
        RuleFor(c => c.Ids).NotEmpty();
    }
}
