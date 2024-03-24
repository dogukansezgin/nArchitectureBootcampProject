using NArchitecture.Core.Application.Dtos;

namespace Application.Features.BootcampImages.Queries.GetList;

public class GetListBootcampImageListItemDto : IDto
{
    public Guid Id { get; set; }
    public Guid BootcampId { get; set; }
    public string ImagePath { get; set; }
}