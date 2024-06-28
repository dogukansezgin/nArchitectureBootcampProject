using System.Linq.Expressions;
using Application.Features.Applications.Rules;
using Application.Features.Applications.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using ApplicationEntity = Domain.Entities.Application;

namespace Application.Services.Applications;

public class ApplicationManager : IApplicationService
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly ApplicationBusinessRules _applicationBusinessRules;

    public ApplicationManager(IApplicationRepository applicationRepository, ApplicationBusinessRules applicationBusinessRules)
    {
        _applicationRepository = applicationRepository;
        _applicationBusinessRules = applicationBusinessRules;
    }

    public async Task<ApplicationEntity?> GetAsync(
        Expression<Func<ApplicationEntity, bool>> predicate,
        Func<IQueryable<ApplicationEntity>, IIncludableQueryable<ApplicationEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        ApplicationEntity? application = await _applicationRepository.GetAsync(
            predicate,
            include,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return application;
    }

    public async Task<IPaginate<ApplicationEntity>?> GetListAsync(
        Expression<Func<ApplicationEntity, bool>>? predicate = null,
        Func<IQueryable<ApplicationEntity>, IOrderedQueryable<ApplicationEntity>>? orderBy = null,
        Func<IQueryable<ApplicationEntity>, IIncludableQueryable<ApplicationEntity, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<ApplicationEntity> applicationList = await _applicationRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return applicationList;
    }

    public async Task<ApplicationEntity> AddAsync(ApplicationEntity application)
    {
        await _applicationBusinessRules.ApplicationForeignKeysShouldExist(application);
        await _applicationBusinessRules.ApplicationApplicantShouldNotExistInBlacklist(application);
        await _applicationBusinessRules.ApplicationShouldNotExist(application);

        ApplicationEntity addedApplication = await _applicationRepository.AddAsync(application);

        return addedApplication;
    }

    public async Task<ApplicationEntity> UpdateAsync(ApplicationEntity application)
    {
        await _applicationBusinessRules.ApplicationForeignKeysShouldExist(application);
        await _applicationBusinessRules.ApplicationIdShouldExistWhenSelected(application.Id);
        await _applicationBusinessRules.ApplicationShouldNotExist(application);

        ApplicationEntity updatedApplication = await _applicationRepository.UpdateAsync(application);

        return updatedApplication;
    }

    public async Task<ICollection<ApplicationEntity>> UpdateRangeAsync(ICollection<ApplicationEntity> applications)
    {
        foreach (ApplicationEntity application in applications)
        {
            await _applicationBusinessRules.ApplicationShouldExistWhenSelected(application);
            await _applicationBusinessRules.ApplicationForeignKeysShouldExist(application);
            await _applicationBusinessRules.ApplicationShouldNotExist(application);
        }

        ICollection<ApplicationEntity> updatedApplications = await _applicationRepository.UpdateRangeAsync(applications);

        return updatedApplications;
    }

    public async Task<ApplicationEntity> DeleteAsync(ApplicationEntity application, bool permanent = false)
    {
        await _applicationBusinessRules.ApplicationShouldExistWhenSelected(application);

        ApplicationEntity deletedApplication = await _applicationRepository.DeleteAsync(application, permanent);

        return deletedApplication;
    }

    public async Task<ICollection<ApplicationEntity>> DeleteRangeAsync(
        ICollection<ApplicationEntity> applications,
        bool permanent = false
    )
    {
        foreach (ApplicationEntity application in applications)
        {
            await _applicationBusinessRules.ApplicationShouldExistWhenSelected(application);
        }

        ICollection<ApplicationEntity> deletedApplications = await _applicationRepository.DeleteRangeCustomAsync(
            applications,
            permanent
        );

        return deletedApplications;
    }

    public async Task<ApplicationEntity> RestoreAsync(ApplicationEntity application)
    {
        await _applicationBusinessRules.ApplicationShouldExistWhenSelected(application);

        ApplicationEntity restoredApplication = await _applicationRepository.RestoreAsync(application);

        return restoredApplication;
    }

    public async Task<ICollection<ApplicationEntity>> RestoreRangeAsync(ICollection<ApplicationEntity> applications)
    {
        foreach (ApplicationEntity application in applications)
        {
            await _applicationBusinessRules.ApplicationShouldExistWhenSelected(application);
        }

        ICollection<ApplicationEntity> deletedApplications = await _applicationRepository.RestoreRangeCustomAsync(applications);

        return deletedApplications;
    }

    public async Task<ApplicationEntity> GetByIdAsync(Guid id)
    {
        ApplicationEntity? application = await _applicationRepository.GetAsync(x => x.Id == id);

        await _applicationBusinessRules.ApplicationShouldExistWhenSelected(application);

        return application;
    }
}
