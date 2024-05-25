using NArchitecture.Core.Application.Responses;

namespace Application.Features.Applications.Commands.DeleteRange;

public class DeletedRangeApplicationResponse : IResponse
{
    public ICollection<Guid> Ids { get; set; }
    public DateTime DeletedDate { get; set; }
    public bool IsPermament { get; set; }
}
