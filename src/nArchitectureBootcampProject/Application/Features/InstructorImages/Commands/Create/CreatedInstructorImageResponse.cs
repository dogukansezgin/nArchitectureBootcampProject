using NArchitecture.Core.Application.Responses;

namespace Application.Features.InstructorImages.Commands.Create;

public class CreatedInstructorImageResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid InstructorId { get; set; }
    public string ImagePath { get; set; }
}