using System.Text.RegularExpressions;
using FluentValidation;

namespace Application.Features.Applicants.Commands.UpdatePasswordFromAuth;

public class UpdateApplicantPasswordFromAuthCommandValidator : AbstractValidator<UpdateApplicantPasswordFromAuthCommand>
{
    public UpdateApplicantPasswordFromAuthCommandValidator()
    {
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(8)
            .Must(StrongPassword)
            .WithMessage(
                "Password must contain at least one uppercase letter, one lowercase letter, one number and one special character."
            );
        RuleFor(x => x.CurrentPassword).NotEmpty();
    }

    private bool StrongPassword(string value)
    {
        Regex strongPasswordRegex = new("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", RegexOptions.Compiled);

        return strongPasswordRegex.IsMatch(value);
    }
}
