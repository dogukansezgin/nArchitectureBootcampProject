using Application.Services.Repositories;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;
using ApplicationEntity = Domain.Entities.Application;

namespace Persistence.Repositories;

public class ApplicationRepository : EfRepositoryBase<ApplicationEntity, Guid, BaseDbContext>, IApplicationRepository
{
    public ApplicationRepository(BaseDbContext context)
        : base(context) { }
}
