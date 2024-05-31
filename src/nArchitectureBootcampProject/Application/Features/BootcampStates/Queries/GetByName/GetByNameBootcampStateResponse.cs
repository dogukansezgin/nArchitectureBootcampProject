using NArchitecture.Core.Application.Responses;

namespace Application.Features.BootcampStates.Queries.GetByName;

public class GetByNameBootcampStateResponse : IResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
