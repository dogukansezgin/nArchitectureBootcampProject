﻿using FluentValidation;
using System.Text.RegularExpressions;

namespace Application.Features.Auth.Commands.Register.ApplicantRegister;
public class ApplicantRegisterCommandValidator : AbstractValidator<ApplicantRegisterCommand>
{
    public ApplicantRegisterCommandValidator()
    {
        RuleFor(c => c.ApplicantForRegisterDto.Email).NotEmpty().EmailAddress();
        RuleFor(c => c.ApplicantForRegisterDto.Password)
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