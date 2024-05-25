using NArchitecture.Core.Application.Responses;

namespace Application.Features.Applications.Commands.Restore;

public class RestoredApplicationResponse : IResponse
{
    public Guid Id { get; set; }
}
