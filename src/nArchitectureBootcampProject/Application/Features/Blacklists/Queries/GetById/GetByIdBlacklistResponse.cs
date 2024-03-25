using NArchitecture.Core.Application.Responses;

namespace Application.Features.Blacklists.Queries.GetById;

public class GetByIdBlacklistResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid ApplicantId { get; set; }
    public string Reason { get; set; }
    public DateTime Date { get; set; }
}
