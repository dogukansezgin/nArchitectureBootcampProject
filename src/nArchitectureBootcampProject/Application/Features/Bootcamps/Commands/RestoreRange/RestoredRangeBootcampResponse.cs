using NArchitecture.Core.Application.Responses;

namespace Application.Features.Bootcamps.Commands.RestoreRange;

public class RestoredRangeBootcampResponse : IResponse
{
    public ICollection<Guid> Ids { get; set; }
}
