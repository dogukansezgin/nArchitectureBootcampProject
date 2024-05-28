using NArchitecture.Core.Application.Responses;

namespace Application.Features.Blacklists.Commands.Restore;

public class RestoredBlacklistResponse : IResponse
{
    public Guid Id { get; set; }
}
