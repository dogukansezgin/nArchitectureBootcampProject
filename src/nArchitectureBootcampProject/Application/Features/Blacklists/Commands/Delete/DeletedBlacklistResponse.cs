using NArchitecture.Core.Application.Responses;

namespace Application.Features.Blacklists.Commands.Delete;

public class DeletedBlacklistResponse : IResponse
{
    public Guid Id { get; set; }
}
