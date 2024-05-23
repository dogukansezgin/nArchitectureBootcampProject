using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Applications.Queries.GetListByUserName;

public class GetListByUserNameApplicationListItemDto : IDto
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }

    public Guid ApplicantId { get; set; }
    public string ApplicantUserName { get; set; }

    public Guid BootcampId { get; set; }
    public string BootcampName { get; set; }

    public Guid ApplicationStateId { get; set; }
    public string ApplicationStateName { get; set; }
}
