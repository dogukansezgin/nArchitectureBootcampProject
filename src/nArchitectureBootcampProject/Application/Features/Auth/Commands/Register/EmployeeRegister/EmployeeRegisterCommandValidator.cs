using System.Text.RegularExpressions;
using FluentValidation;

namespace Application.Features.Auth.Commands.Register.EmployeeRegister;

public class EmployeeRegisterCommandValidator : AbstractValidator<EmployeeRegisterCommand>
{
    public EmployeeRegisterCommandValidator()
    {
        RuleFor(c => c.EmployeeForRegisterDto.Email).NotEmpty().EmailAddress();
        RuleFor(c => c.EmployeeForRegisterDto.Password)
            .NotEmpty()
            .MinimumLength(6)
            .Must(StrongPassword)
            .WithMessage(
                "Password must contain at least one uppercase letter, one lowercase letter, one number and one special character."
            );
    }

    private bool StrongPassword(string value)
    {
        Regex strongPasswordRegex = new("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", RegexOptions.Compiled);

        return strongPasswordRegex.IsMatch(value);
    }
}
