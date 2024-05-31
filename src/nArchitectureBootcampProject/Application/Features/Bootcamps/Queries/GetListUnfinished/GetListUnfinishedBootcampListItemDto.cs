using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Bootcamps.Queries.GetListUnfinished;

public class GetListUnfinishedBootcampListItemDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedDate { get; set; }

    public Guid InstructorId { get; set; }
    public string InstructorUserName { get; set; }
    public string? InstructorFirstName { get; set; }
    public string? InstructorLastName { get; set; }
    public string InstructorCompanyName { get; set; }

    public Guid BootcampStateId { get; set; }
    public string BootcampStateName { get; set; }

    public Guid BootcampImageId { get; set; }
    public string BootcampImagePath { get; set; }
}
