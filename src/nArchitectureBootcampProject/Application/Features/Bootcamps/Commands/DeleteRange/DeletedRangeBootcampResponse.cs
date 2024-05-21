using NArchitecture.Core.Application.Responses;

namespace Application.Features.Bootcamps.Commands.DeleteRange;

public class DeletedRangeBootcampResponse : IResponse
{
    public ICollection<Guid> Ids { get; set; }
    public DateTime DeletedDate { get; set; }
    public bool IsPermament { get; set; }
}
