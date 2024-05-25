using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IApplicantRepository : IAsyncRepository<Applicant, Guid>, IRepository<Applicant, Guid> 
{
    Task<Applicant> RestoreAsync(Applicant applicant);
    Task<ICollection<Applicant>> DeleteRangeCustomAsync(
        ICollection<Applicant> entities,
        bool permanent = false,
        CancellationToken cancellationToken = default
    );
    Task<ICollection<Applicant>> RestoreRangeCustomAsync(ICollection<Applicant> entities);
}
