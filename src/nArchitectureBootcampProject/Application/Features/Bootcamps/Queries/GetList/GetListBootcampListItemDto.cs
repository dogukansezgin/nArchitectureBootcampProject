using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Bootcamps.Queries.GetList;

public class GetListBootcampListItemDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public Guid InstructorId { get; set; }
    public string InstructorUserName { get; set; }
    public string? InstructorFirstName { get; set; }
    public string? InstructorLastName { get; set; }
    public string InstructorCompanyName { get; set; }

    public Guid BootcampStateId { get; set; }
    public string BootcampStateName { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public DateTime CreatedDate { get; set; }
}
