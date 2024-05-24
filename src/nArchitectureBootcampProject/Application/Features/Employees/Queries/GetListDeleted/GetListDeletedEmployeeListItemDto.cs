using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Employees.Queries.GetListDeleted;

public class GetListDeletedEmployeeListItemDto : IDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? NationalIdentity { get; set; }
    public string Position { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime DeletedDate { get; set; }
}
