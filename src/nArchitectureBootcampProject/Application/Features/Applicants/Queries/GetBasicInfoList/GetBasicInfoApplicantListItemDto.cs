using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Applicants.Queries.GetBasicInfoList;

public class GetBasicInfoApplicantListItemDto : IDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
}
