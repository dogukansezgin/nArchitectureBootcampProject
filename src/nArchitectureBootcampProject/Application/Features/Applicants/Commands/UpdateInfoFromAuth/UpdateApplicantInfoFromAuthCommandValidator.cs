using FluentValidation;

namespace Application.Features.Applicants.Commands.UpdateInfoFromAuth;

public class UpdateApplicantInfoFromAuthCommandValidator : AbstractValidator<UpdateApplicantInfoFromAuthCommand>
{
    public UpdateApplicantInfoFromAuthCommandValidator()
    {
        RuleFor(c => c.About).MaximumLength(300).WithMessage("En fazla 300 karakter girmelisin.");
        RuleFor(c => c.FirstName).MinimumLength(3).MaximumLength(30).WithMessage("En fazla 300 karakter girmelisin.");
        RuleFor(c => c.LastName).MinimumLength(2).MaximumLength(30).WithMessage("En fazla 300 karakter girmelisin.");
        RuleFor(c => c.NationalIdentity).Length(11).WithMessage("En fazla 300 karakter girmelisin.");
    }
}
