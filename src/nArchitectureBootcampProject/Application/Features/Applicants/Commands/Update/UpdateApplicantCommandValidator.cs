using FluentValidation;
using System.Text.RegularExpressions;

namespace Application.Features.Applicants.Commands.Update;

public class UpdateApplicantCommandValidator : AbstractValidator<UpdateApplicantCommand>
{
    public UpdateApplicantCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.NationalIdentity).MinimumLength(11).MaximumLength(11);
        RuleFor(c => c.Email)
            .NotEmpty()
            .EmailAddress()
            .Must(BeValidEmail)
            .WithMessage("Email must be a Gmail or Hotmail address.");
    }

    private bool BeValidEmail(string email)
    {
        string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        Regex emailRegex = new(emailPattern, RegexOptions.Compiled);

        if (!emailRegex.IsMatch(email))
        {
            return false;
        }

        if (email.Count(c => c == '@') != 1)
        {
            return false;
        }

        string[] validDomains = { "gmail.com", "hotmail.com" };
        string emailDomain = email.Split('@').Last();

        return validDomains.Contains(emailDomain);
    }
}
