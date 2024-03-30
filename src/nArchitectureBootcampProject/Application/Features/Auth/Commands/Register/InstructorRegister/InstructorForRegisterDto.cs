using Application.Features.Auth.Commands.Register.UserRegister;

namespace Application.Features.Auth.Commands.Register.InstructorRegister;

public class InstructorForRegisterDto : UserForRegisterDto
{
    public string CompanyName { get; set; }
}
