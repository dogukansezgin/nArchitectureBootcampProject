using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Security.JWT;

namespace Application.Features.Auth.Commands.Register.ApplicantRegister;
public class ApplicantRegisteredResponse : IResponse
{
    public AccessToken AccessToken { get; set; }
    public Domain.Entities.RefreshToken RefreshToken { get; set; }

    public ApplicantRegisteredResponse()
    {
        AccessToken = null!;
        RefreshToken = null!;
    }

    public ApplicantRegisteredResponse(AccessToken accessToken, Domain.Entities.RefreshToken refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}