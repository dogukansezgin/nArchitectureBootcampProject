using NArchitecture.Core.Application.Dtos;

namespace Application.Features.ApplicationStates.Queries.GetListDeleted;

public class GetListDeletedApplicationStateListItemDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime DeletedDate { get; set; }
}
