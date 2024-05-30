using NArchitecture.Core.Application.Responses;

namespace Application.Features.Instructors.Queries.GetBasicInfoById;

public class GetBasicInfoByIdInstructorResponse : IResponse
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string CompanyName { get; set; }
}
