using NArchitecture.Core.Application.Responses;

namespace Application.Features.Blacklists.Queries.GetByApplicantId;

public class GetByApplicantIdBlacklistResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid ApplicantId { get; set; }
    public string Reason { get; set; }
    public DateTime Date { get; set; }
    public DateTime CreatedDate { get; set; }
}
