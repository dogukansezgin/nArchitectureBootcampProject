using NArchitecture.Core.Application.Responses;

namespace Application.Features.BootcampImages.Commands.Update;

public class UpdatedBootcampImageResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid BootcampId { get; set; }
    public string ImagePath { get; set; }
}