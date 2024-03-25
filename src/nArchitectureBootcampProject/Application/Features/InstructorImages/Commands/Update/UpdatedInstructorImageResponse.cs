using NArchitecture.Core.Application.Responses;

namespace Application.Features.InstructorImages.Commands.Update;

public class UpdatedInstructorImageResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid InstructorId { get; set; }
    public string ImagePath { get; set; }
}
