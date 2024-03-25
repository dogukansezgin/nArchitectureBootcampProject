using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class BootcampImageRepository : EfRepositoryBase<BootcampImage, Guid, BaseDbContext>, IBootcampImageRepository
{
    public BootcampImageRepository(BaseDbContext context)
        : base(context) { }
}
