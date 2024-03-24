using NArchitecture.Core.Application.Responses;

namespace Application.Features.Blacklists.Commands.Create;

public class CreatedBlacklistResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid ApplicantId { get; set; }
    public string Reason { get; set; }
    public DateTime Date { get; set; }
}