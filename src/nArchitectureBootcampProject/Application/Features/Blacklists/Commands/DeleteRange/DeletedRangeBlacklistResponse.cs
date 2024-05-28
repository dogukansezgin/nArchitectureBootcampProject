using NArchitecture.Core.Application.Responses;

namespace Application.Features.Blacklists.Commands.DeleteRange;

public class DeletedRangeBlacklistResponse : IResponse
{
    public ICollection<Guid> Ids { get; set; }
    public DateTime DeletedDate { get; set; }
    public bool IsPermament { get; set; }
}
