using System.Linq.Expressions;
using Application.Features.Blacklists.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Services.Blacklists;

public class BlacklistManager : IBlacklistService
{
    private readonly IBlacklistRepository _blacklistRepository;
    private readonly BlacklistBusinessRules _blacklistBusinessRules;

    public BlacklistManager(IBlacklistRepository blacklistRepository, BlacklistBusinessRules blacklistBusinessRules)
    {
        _blacklistRepository = blacklistRepository;
        _blacklistBusinessRules = blacklistBusinessRules;
    }

    public async Task<Blacklist?> GetAsync(
        Expression<Func<Blacklist, bool>> predicate,
        Func<IQueryable<Blacklist>, IIncludableQueryable<Blacklist, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        Blacklist? blacklist = await _blacklistRepository.GetAsync(
            predicate,
            include,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return blacklist;
    }

    public async Task<IPaginate<Blacklist>?> GetListAsync(
        Expression<Func<Blacklist, bool>>? predicate = null,
        Func<IQueryable<Blacklist>, IOrderedQueryable<Blacklist>>? orderBy = null,
        Func<IQueryable<Blacklist>, IIncludableQueryable<Blacklist, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<Blacklist> blacklistList = await _blacklistRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return blacklistList;
    }

    public async Task<Blacklist> AddAsync(Blacklist blacklist)
    {
        await _blacklistBusinessRules.BlacklistForeignKeysShouldExist(blacklist);
        await _blacklistBusinessRules.BlacklistApplicantCheck(blacklist.Id);

        Blacklist addedBlacklist = await _blacklistRepository.AddAsync(blacklist);

        return addedBlacklist;
    }

    public async Task<Blacklist> UpdateAsync(Blacklist blacklist)
    {
        await _blacklistBusinessRules.BlacklistForeignKeysShouldExist(blacklist);
        await _blacklistBusinessRules.BlacklistApplicantCheck(blacklist.Id);
        await _blacklistBusinessRules.BlacklistIdShouldExistWhenSelected(blacklist.Id);

        Blacklist updatedBlacklist = await _blacklistRepository.UpdateAsync(blacklist);

        return updatedBlacklist;
    }

    public async Task<Blacklist> DeleteAsync(Blacklist blacklist, bool permanent = false)
    {
        await _blacklistBusinessRules.BlacklistShouldExistWhenSelected(blacklist);

        Blacklist deletedBlacklist = await _blacklistRepository.DeleteAsync(blacklist, permanent);

        return deletedBlacklist;
    }

    public async Task<Blacklist> GetByIdAsync(Guid id)
    {
        Blacklist? blacklist = await _blacklistRepository.GetAsync(x => x.Id == id);

        await _blacklistBusinessRules.BlacklistShouldExistWhenSelected(blacklist);

        return blacklist;
    }
}
