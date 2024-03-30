namespace Application.Features.Auth.Commands.Register.UserRegister;

public class UserForRegisterDto : NArchitecture.Core.Application.Dtos.UserForRegisterDto
{
    public string UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? NationalIdentity { get; set; }
}
