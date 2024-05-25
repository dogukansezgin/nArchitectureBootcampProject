using NArchitecture.Core.Application.Dtos;

namespace Application.Features.BootcampStates.Queries.GetListDeleted;

public class GetListDeletedBootcampStateListItemDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime DeletedDate { get; set; }
}
