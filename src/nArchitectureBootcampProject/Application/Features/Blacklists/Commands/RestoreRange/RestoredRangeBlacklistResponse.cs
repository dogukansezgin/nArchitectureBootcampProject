using NArchitecture.Core.Application.Responses;

namespace Application.Features.Blacklists.Commands.RestoreRange;

public class RestoredRangeBlacklistResponse : IResponse
{
    public ICollection<Guid> Ids { get; set; }
}
