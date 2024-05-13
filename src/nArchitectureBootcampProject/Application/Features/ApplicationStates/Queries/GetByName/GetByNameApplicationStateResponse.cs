using NArchitecture.Core.Application.Responses;

namespace Application.Features.ApplicationStates.Queries.GetByName;

public class GetByNameApplicationStateResponse : IResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
