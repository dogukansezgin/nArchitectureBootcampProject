using NArchitecture.Core.Application.Responses;

namespace Application.Features.Applications.Commands.RestoreRange;

public class RestoredRangeApplicationResponse : IResponse
{
    public ICollection<Guid> Ids { get; set; }
}
