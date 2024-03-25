using NArchitecture.Core.Application.Responses;

namespace Application.Features.Applications.Queries.GetById;

public class GetByIdApplicationResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid ApplicantId { get; set; }
    public Guid BootcampId { get; set; }
    public Guid ApplicationStateId { get; set; }
}
