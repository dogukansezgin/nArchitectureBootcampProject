using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Security.JWT;

namespace Application.Features.Applicants.Commands.UpdateInfoFromAuth;
public class UpdatedApplicantInfoFromAuthResponse : IResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string NationalIdentity { get; set; }
    public string? About { get; set; }
    public AccessToken AccessToken { get; set; }
}
