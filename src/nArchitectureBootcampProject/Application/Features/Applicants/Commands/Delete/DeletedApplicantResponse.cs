using NArchitecture.Core.Application.Responses;

namespace Application.Features.Applicants.Commands.Delete;

public class DeletedApplicantResponse : IResponse
{
    public Guid Id { get; set; }
    public DateTime DeletedDate { get; set; }
    public bool IsPermament { get; set; }
}
