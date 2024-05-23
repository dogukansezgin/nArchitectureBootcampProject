using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Applications.Queries.GetListByState;

public class GetListApplicationByStateListItemDto : IDto
{
    public Guid Id { get; set; }
    public Guid ApplicantId { get; set; }
    public Guid BootcampId { get; set; }
    public Guid ApplicationStateId { get; set; }

    public DateTime CreatedDate { get; set; }

    public string ApplicantFirstName { get; set; }
    public string ApplicantLastName { get; set; }

    public string BootcampName { get; set; }

    public string ApplicationStateName { get; set; }
}
