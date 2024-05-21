using NArchitecture.Core.Application.Responses;

namespace Application.Features.Bootcamps.Commands.Delete;

public class DeletedBootcampResponse : IResponse
{
    public Guid Id { get; set; }
    public DateTime DeletedDate { get; set; }
    public bool IsPermament { get; set; }
}
