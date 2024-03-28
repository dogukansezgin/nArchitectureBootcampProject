using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Security.JWT;

namespace Application.Features.Auth.Commands.Register.InstructorRegister;
public class InstructorRegisteredResponse : IResponse
{
    public AccessToken AccessToken { get; set; }
    public Domain.Entities.RefreshToken RefreshToken { get; set; }

    public InstructorRegisteredResponse()
    {
        AccessToken = null!;
        RefreshToken = null!;
    }

    public InstructorRegisteredResponse(AccessToken accessToken, Domain.Entities.RefreshToken refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}