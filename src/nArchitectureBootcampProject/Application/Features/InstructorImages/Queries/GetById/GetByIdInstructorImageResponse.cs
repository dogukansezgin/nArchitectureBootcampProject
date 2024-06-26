using NArchitecture.Core.Application.Responses;

namespace Application.Features.InstructorImages.Queries.GetById;

public class GetByIdInstructorImageResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid InstructorId { get; set; }
    public string ImagePath { get; set; }
}
