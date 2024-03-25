using NArchitecture.Core.Application.Responses;

namespace Application.Features.BootcampImages.Queries.GetById;

public class GetByIdBootcampImageResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid BootcampId { get; set; }
    public string ImagePath { get; set; }
}
