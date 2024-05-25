using NArchitecture.Core.Application.Responses;

namespace Application.Features.Applicants.Commands.Restore;

public class RestoredApplicantResponse : IResponse
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
}
