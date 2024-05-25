using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Bootcamps.Queries.GetBasicInfoList;

public class GetBasicInfoBootcampListItemDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
