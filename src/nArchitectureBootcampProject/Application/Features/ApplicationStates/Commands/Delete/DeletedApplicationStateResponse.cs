using NArchitecture.Core.Application.Responses;

namespace Application.Features.ApplicationStates.Commands.Delete;

public class DeletedApplicationStateResponse : IResponse
{
    public Guid Id { get; set; }
    public DateTime DeletedDate { get; set; }
    public bool IsPermament { get; set; }
}
