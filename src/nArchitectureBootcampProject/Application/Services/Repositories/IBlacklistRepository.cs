using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IBlacklistRepository : IAsyncRepository<Blacklist, Guid>, IRepository<Blacklist, Guid> { }
