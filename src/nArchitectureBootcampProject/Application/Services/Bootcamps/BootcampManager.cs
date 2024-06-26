using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using Application.Features.Bootcamps.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Services.Bootcamps;

public class BootcampManager : IBootcampService
{
    private readonly IBootcampRepository _bootcampRepository;
    private readonly BootcampBusinessRules _bootcampBusinessRules;

    public BootcampManager(IBootcampRepository bootcampRepository, BootcampBusinessRules bootcampBusinessRules)
    {
        _bootcampRepository = bootcampRepository;
        _bootcampBusinessRules = bootcampBusinessRules;
    }

    public async Task<Bootcamp?> GetAsync(
        Expression<Func<Bootcamp, bool>> predicate,
        Func<IQueryable<Bootcamp>, IIncludableQueryable<Bootcamp, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        Bootcamp? bootcamp = await _bootcampRepository.GetAsync(
            predicate,
            include,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return bootcamp;
    }

    public async Task<IPaginate<Bootcamp>?> GetListAsync(
        Expression<Func<Bootcamp, bool>>? predicate = null,
        Func<IQueryable<Bootcamp>, IOrderedQueryable<Bootcamp>>? orderBy = null,
        Func<IQueryable<Bootcamp>, IIncludableQueryable<Bootcamp, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<Bootcamp> bootcampList = await _bootcampRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return bootcampList;
    }

    public async Task<Bootcamp> AddAsync(Bootcamp bootcamp)
    {
        await _bootcampBusinessRules.BootcampForeignKeysShouldExist(bootcamp);
        await _bootcampBusinessRules.BootcampShouldNotExist(bootcamp);

        Bootcamp addedBootcamp = await _bootcampRepository.AddAsync(bootcamp);

        return addedBootcamp;
    }

    public async Task<Bootcamp> UpdateAsync(Bootcamp bootcamp)
    {
        await _bootcampBusinessRules.BootcampForeignKeysShouldExist(bootcamp);
        await _bootcampBusinessRules.BootcampIdShouldExistWhenSelected(bootcamp.Id);
        await _bootcampBusinessRules.BootcampShouldNotExist(bootcamp);

        Bootcamp updatedBootcamp = await _bootcampRepository.UpdateAsync(bootcamp);

        return updatedBootcamp;
    }

    public async Task<Bootcamp> DeleteAsync(Bootcamp bootcamp, bool permanent = false)
    {
        await _bootcampBusinessRules.BootcampShouldExistWhenSelected(bootcamp);

        Bootcamp deletedBootcamp = await _bootcampRepository.DeleteAsync(bootcamp, permanent);

        return deletedBootcamp;
    }

    public async Task<ICollection<Bootcamp>> DeleteRangeAsync(ICollection<Bootcamp> bootcamps, bool permanent = false)
    {
        foreach (Bootcamp bootcamp in bootcamps)
        {
            await _bootcampBusinessRules.BootcampShouldExistWhenSelected(bootcamp);
        }

        ICollection<Bootcamp> deletedBootcamps = await _bootcampRepository.DeleteRangeCustomAsync(bootcamps, permanent);

        return deletedBootcamps;
    }

    public async Task<Bootcamp> RestoreAsync(Bootcamp bootcamp)
    {
        await _bootcampBusinessRules.BootcampShouldExistWhenSelected(bootcamp);

        Bootcamp restoredBootcamp = await _bootcampRepository.RestoreAsync(bootcamp);

        return restoredBootcamp;
    }

    public async Task<ICollection<Bootcamp>> RestoreRangeAsync(ICollection<Bootcamp> bootcamps)
    {
        foreach (Bootcamp bootcamp in bootcamps)
        {
            await _bootcampBusinessRules.BootcampShouldExistWhenSelected(bootcamp);
        }

        ICollection<Bootcamp> deletedBootcamps = await _bootcampRepository.RestoreRangeCustomAsync(bootcamps);

        return deletedBootcamps;
    }

    public async Task<Bootcamp> GetByIdAsync(Guid id)
    {
        Bootcamp? bootcamp = await _bootcampRepository.GetAsync(
            x => x.Id == id,
            include: x => x.Include(x => x.Instructor).Include(x => x.BootcampState)
        );

        await _bootcampBusinessRules.BootcampShouldExistWhenSelected(bootcamp);

        return bootcamp;
    }

    public async Task<Bootcamp> GetByNameAsync(string name)
    {
        Bootcamp? bootcamp = await _bootcampRepository.GetAsync(
            x => EF.Functions.Collate(x.Name.ToLower(), "SQL_Latin1_General_CP1253_CI_AI").Contains(name.ToLower()),
            include: x => x.Include(x => x.Instructor).Include(x => x.BootcampState)
        );

        await _bootcampBusinessRules.BootcampShouldExistWhenSelected(bootcamp);

        return bootcamp;
    }
}
