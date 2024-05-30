namespace Application.Features.Auth.Commands.Register.UserRegister;

public class UserForRegisterDto : NArchitecture.Core.Application.Dtos.UserForRegisterDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
