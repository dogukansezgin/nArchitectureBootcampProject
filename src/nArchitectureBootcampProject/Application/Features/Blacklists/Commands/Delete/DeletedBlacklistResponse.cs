using NArchitecture.Core.Application.Responses;

namespace Application.Features.Blacklists.Commands.Delete;

public class DeletedBlacklistResponse : IResponse
{
    public Guid Id { get; set; }
    public DateTime DeletedDate { get; set; }
    public bool IsPermament { get; set; }
}
