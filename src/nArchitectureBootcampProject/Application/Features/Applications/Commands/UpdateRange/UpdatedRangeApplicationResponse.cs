using NArchitecture.Core.Application.Responses;

namespace Application.Features.Applications.Commands.UpdateRange;

public class UpdatedRangeApplicationResponse : IResponse
{
    public ICollection<Guid> Ids { get; set; }
    public DateTime UpdatedDate { get; set; }
}
