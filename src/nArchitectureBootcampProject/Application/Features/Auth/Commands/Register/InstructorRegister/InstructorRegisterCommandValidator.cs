using FluentValidation;
using System.Text.RegularExpressions;

namespace Application.Features.Auth.Commands.Register.InstructorRegister;
public class InstructorRegisterCommandValidator : AbstractValidator<InstructorRegisterCommand>
{
    public InstructorRegisterCommandValidator()
    {
        RuleFor(c => c.InstructorForRegisterDto.Email).NotEmpty().EmailAddress();
        RuleFor(c => c.InstructorForRegisterDto.Password)
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
