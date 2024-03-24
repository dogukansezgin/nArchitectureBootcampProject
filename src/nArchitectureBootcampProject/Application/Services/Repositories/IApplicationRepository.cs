using ApplicationEntity = Domain.Entities.Application;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IApplicationRepository : IAsyncRepository<ApplicationEntity, Guid>, IRepository<ApplicationEntity, Guid>
{
}