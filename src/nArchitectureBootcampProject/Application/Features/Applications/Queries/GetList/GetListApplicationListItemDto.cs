using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Applications.Queries.GetList;

public class GetListApplicationListItemDto : IDto
{
    public Guid Id { get; set; }

    public Guid ApplicantId { get; set; }
    public string ApplicantUserName { get; set; }
    public string ApplicantEmail { get; set; }

    public Guid BootcampId { get; set; }
    public string BootcampName { get; set; }

    public Guid ApplicationStateId { get; set; }
    public string ApplicationStateName { get; set; }

    public DateTime CreatedDate { get; set; }
}
