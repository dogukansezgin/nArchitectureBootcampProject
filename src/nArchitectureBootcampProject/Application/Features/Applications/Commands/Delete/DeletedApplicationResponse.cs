using NArchitecture.Core.Application.Responses;

namespace Application.Features.Applications.Commands.Delete;

public class DeletedApplicationResponse : IResponse
{
    public Guid Id { get; set; }
    public bool IsPermament { get; set; }
}
