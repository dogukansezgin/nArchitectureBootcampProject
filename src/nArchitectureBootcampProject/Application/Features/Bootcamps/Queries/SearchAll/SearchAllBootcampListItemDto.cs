using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Bootcamps.Queries.SearchAll;

public class SearchAllBootcampListItemDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
