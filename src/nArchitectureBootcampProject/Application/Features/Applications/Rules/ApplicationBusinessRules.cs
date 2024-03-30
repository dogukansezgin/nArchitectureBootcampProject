using Application.Features.Applicants.Rules;
using Application.Features.Applications.Constants;
using Application.Features.ApplicationStates.Rules;
using Application.Features.Blacklists.Rules;
using Application.Features.Bootcamps.Rules;
using Application.Services.Repositories;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;
using ApplicationEntity = Domain.Entities.Application;

namespace Application.Features.Applications.Rules;

public class ApplicationBusinessRules : BaseBusinessRules
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly ILocalizationService _localizationService;
    private readonly ApplicantBusinessRules _applicantBusinessRules;
    private readonly ApplicationStateBusinessRules _applicationStateBusinessRules;
    private readonly BootcampBusinessRules _bootcampBusinessRules;
    private readonly BlacklistBusinessRules _blacklistBusinessRules;

    public ApplicationBusinessRules(
        IApplicationRepository applicationRepository,
        ILocalizationService localizationService,
        ApplicantBusinessRules applicantBusinessRules,
        ApplicationStateBusinessRules applicationStateBusinessRules,
        BootcampBusinessRules bootcampBusinessRules,
        BlacklistBusinessRules blacklistBusinessRules
    )
    {
        _applicationRepository = applicationRepository;
        _localizationService = localizationService;
        _applicantBusinessRules = applicantBusinessRules;
        _applicationStateBusinessRules = applicationStateBusinessRules;
        _bootcampBusinessRules = bootcampBusinessRules;
        _blacklistBusinessRules = blacklistBusinessRules;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, ApplicationsBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task ApplicationShouldExistWhenSelected(ApplicationEntity? application)
    {
        if (application == null)
            await throwBusinessException(ApplicationsBusinessMessages.ApplicationNotExists);
    }

    public async Task ApplicationIdShouldExistWhenSelected(Guid id)
    {
        ApplicationEntity? application = await _applicationRepository.GetAsync(predicate: a => a.Id == id, enableTracking: false);
        await ApplicationShouldExistWhenSelected(application);
    }

    public async Task ApplicationForeignKeysShouldExist(ApplicationEntity? application)
    {
        await ApplicationShouldExistWhenSelected(application);

        await _applicantBusinessRules.ApplicantIdShouldExistWhenSelected(application.ApplicantId);
        await _applicationStateBusinessRules.ApplicationStateIdShouldExistWhenSelected(application.ApplicationStateId);
        await _bootcampBusinessRules.BootcampIdShouldExistWhenSelected(application.BootcampId);
    }

    public async Task ApplicationShouldNotExist(ApplicationEntity? application)
    {
        var isExistApplicantId = _applicationRepository.Get(x => x.ApplicantId == application.ApplicantId) is not null;
        var isExistBootcampId = _applicationRepository.Get(x => x.BootcampId == application.BootcampId) is not null;

        if (isExistApplicantId && isExistBootcampId)
            await throwBusinessException(ApplicationsBusinessMessages.ApplicationExists);
    }

    public async Task ApplicationApplicantShouldNotExistInBlacklist(ApplicationEntity? application)
    {
        await _blacklistBusinessRules.BlacklistApplicantCheck(application.ApplicantId);
    }
}
