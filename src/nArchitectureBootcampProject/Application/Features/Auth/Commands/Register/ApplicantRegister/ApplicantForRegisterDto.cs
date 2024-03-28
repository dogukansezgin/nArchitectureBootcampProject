using Application.Features.Auth.Commands.Register.UserRegister;

namespace Application.Features.Auth.Commands.Register.ApplicantRegister;
public class ApplicantForRegisterDto : UserForRegisterDto
{
    public string About { get; set; }
}
