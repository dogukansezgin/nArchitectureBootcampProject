using NArchitecture.Core.Application.Responses;

namespace Application.Features.ApplicationStates.Commands.Restore;
public class RestoredApplicationStateResponse : IResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}