using NArchitecture.Core.Application.Responses;

namespace Application.Features.BootcampStates.Commands.Delete;

public class DeletedBootcampStateResponse : IResponse
{
    public Guid Id { get; set; }
    public DateTime DeletedDate { get; set; }
    public bool IsPermament { get; set; }
}
