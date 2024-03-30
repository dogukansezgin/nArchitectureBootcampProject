using Application.Features.Auth.Commands.Register.UserRegister;

namespace Application.Features.Auth.Commands.Register.EmployeeRegister;

public class EmployeeForRegisterDto : UserForRegisterDto
{
    public string Position { get; set; }
}
