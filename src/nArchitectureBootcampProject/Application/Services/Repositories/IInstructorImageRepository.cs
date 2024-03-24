using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IInstructorImageRepository : IAsyncRepository<InstructorImage, Guid>, IRepository<InstructorImage, Guid>
{
}