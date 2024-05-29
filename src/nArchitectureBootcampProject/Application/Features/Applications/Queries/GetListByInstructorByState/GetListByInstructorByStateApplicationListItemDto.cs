using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Applications.Queries.GetListByInstructorByState;

public class GetListByInstructorByStateApplicationListItemDto : IDto
{
    public Guid Id { get; set; }

    public Guid ApplicantId { get; set; }
    public string ApplicantUserName { get; set; }
    public string ApplicantEmail { get; set; }

    public Guid BootcampId { get; set; }
    public string BootcampName { get; set; }
    public Guid BootcampInstructorId { get; set; }
    public string BootcampInstructorUserName { get; set; }

    public Guid ApplicationStateId { get; set; }
    public string ApplicationStateName { get; set; }

    public DateTime CreatedDate { get; set; }
}
