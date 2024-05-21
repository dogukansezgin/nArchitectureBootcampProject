using NArchitecture.Core.Application.Responses;

namespace Application.Features.Bootcamps.Commands.Restore;

public class RestoredBootcampResponse : IResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
