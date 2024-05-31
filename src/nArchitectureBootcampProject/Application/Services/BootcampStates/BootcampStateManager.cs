using System.Linq.Expressions;
using Application.Features.BootcampStates.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Services.BootcampStates;

public class BootcampStateManager : IBootcampStateService
{
    private readonly IBootcampStateRepository _bootcampStateRepository;
    private readonly BootcampStateBusinessRules _bootcampStateBusinessRules;

    public BootcampStateManager(
        IBootcampStateRepository bootcampStateRepository,
        BootcampStateBusinessRules bootcampStateBusinessRules
    )
    {
        _bootcampStateRepository = bootcampStateRepository;
        _bootcampStateBusinessRules = bootcampStateBusinessRules;
    }

    public async Task<BootcampState?> GetAsync(
        Expression<Func<BootcampState, bool>> predicate,
        Func<IQueryable<BootcampState>, IIncludableQueryable<BootcampState, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        BootcampState? bootcampState = await _bootcampStateRepository.GetAsync(
            predicate,
            include,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return bootcampState;
    }

    public async Task<IPaginate<BootcampState>?> GetListAsync(
        Expression<Func<BootcampState, bool>>? predicate = null,
        Func<IQueryable<BootcampState>, IOrderedQueryable<BootcampState>>? orderBy = null,
        Func<IQueryable<BootcampState>, IIncludableQueryable<BootcampState, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<BootcampState> bootcampStateList = await _bootcampStateRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return bootcampStateList;
    }

    public async Task<BootcampState> AddAsync(BootcampState bootcampState)
    {
        await _bootcampStateBusinessRules.BootcampStateShouldNotExist(bootcampState);

        BootcampState addedBootcampState = await _bootcampStateRepository.AddAsync(bootcampState);

        return addedBootcampState;
    }

    public async Task<BootcampState> UpdateAsync(BootcampState bootcampState)
    {
        await _bootcampStateBusinessRules.BootcampStateShouldExistWhenSelected(bootcampState);
        await _bootcampStateBusinessRules.BootcampStateIdShouldExistWhenSelected(bootcampState.Id);
        await _bootcampStateBusinessRules.BootcampStateShouldNotExistWhenUpdate(bootcampState);

        BootcampState updatedBootcampState = await _bootcampStateRepository.UpdateAsync(bootcampState);

        return updatedBootcampState;
    }

    public async Task<BootcampState> DeleteAsync(BootcampState bootcampState, bool permanent = false)
    {
        await _bootcampStateBusinessRules.BootcampStateShouldExistWhenSelected(bootcampState);

        BootcampState deletedBootcampState = await _bootcampStateRepository.DeleteAsync(bootcampState, permanent);

        return deletedBootcampState;
    }

    public async Task<ICollection<BootcampState>> DeleteRangeAsync(
        ICollection<BootcampState> bootcampstates,
        bool permanent = false
    )
    {
        foreach (BootcampState bootcampstate in bootcampstates)
        {
            await _bootcampStateBusinessRules.BootcampStateShouldExistWhenSelected(bootcampstate);
        }

        ICollection<BootcampState> deletedBootcampStates = await _bootcampStateRepository.DeleteRangeCustomAsync(
            bootcampstates,
            permanent
        );

        return deletedBootcampStates;
    }

    public async Task<BootcampState> RestoreAsync(BootcampState bootcampstate)
    {
        await _bootcampStateBusinessRules.BootcampStateShouldExistWhenSelected(bootcampstate);

        BootcampState restoredBootcampState = await _bootcampStateRepository.RestoreAsync(bootcampstate);

        return restoredBootcampState;
    }

    public async Task<ICollection<BootcampState>> RestoreRangeAsync(ICollection<BootcampState> bootcampstates)
    {
        foreach (BootcampState bootcampstate in bootcampstates)
        {
            await _bootcampStateBusinessRules.BootcampStateShouldExistWhenSelected(bootcampstate);
        }

        ICollection<BootcampState> deletedBootcampStates = await _bootcampStateRepository.RestoreRangeCustomAsync(bootcampstates);

        return deletedBootcampStates;
    }

    public async Task<BootcampState> GetByIdAsync(Guid id)
    {
        BootcampState? bootcampState = await _bootcampStateRepository.GetAsync(x => x.Id == id);

        await _bootcampStateBusinessRules.BootcampStateShouldExistWhenSelected(bootcampState);

        return bootcampState;
    }

    public async Task<BootcampState> GetByNameAsync(string name)
    {
        BootcampState? bootcampState = await _bootcampStateRepository.GetAsync(x => x.Name == name);

        await _bootcampStateBusinessRules.BootcampStateShouldExistWhenSelected(bootcampState);

        return bootcampState;
    }
}
