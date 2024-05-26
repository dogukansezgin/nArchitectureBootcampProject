using System.Text.RegularExpressions;
using FluentValidation;

namespace Application.Features.Employees.Commands.Update;

public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Position).NotEmpty();
        RuleFor(c => c.NationalIdentity)
            .Length(11).When(c => c.NationalIdentity != null && c.NationalIdentity.Length > 0);
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
