using NArchitecture.Core.Application.Responses;

namespace Application.Features.Instructors.Commands.Restore;
public class RestoredInstructorResponse : IResponse
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
}