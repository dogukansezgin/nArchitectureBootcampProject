using NArchitecture.Core.Application.Responses;

namespace Application.Features.BootcampStates.Commands.Restore;
public class RestoredBootcampStateResponse : IResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}