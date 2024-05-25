using Application.Features.ApplicationStates.Constants;
using Application.Features.ApplicationStates.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.ApplicationStates.Rules;

public class ApplicationStateBusinessRules : BaseBusinessRules
{
    private readonly IApplicationStateRepository _applicationStateRepository;
    private readonly ILocalizationService _localizationService;

    public ApplicationStateBusinessRules(
        IApplicationStateRepository applicationStateRepository,
        ILocalizationService localizationService
    )
    {
        _applicationStateRepository = applicationStateRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, ApplicationStatesBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task ApplicationStateShouldExistWhenSelected(ApplicationState? applicationState)
    {
        if (applicationState == null)
            await throwBusinessException(ApplicationStatesBusinessMessages.ApplicationStateNotExists);
    }

    public async Task ApplicationStateIdShouldExistWhenSelected(Guid id)
    {
        ApplicationState? applicationState = await _applicationStateRepository.GetAsync(
            predicate: a => a.Id == id,
            enableTracking: false
        );
        await ApplicationStateShouldExistWhenSelected(applicationState);
    }

    public async Task ApplicationStateShouldNotExist(ApplicationState applicationState)
    {
        var isExistName =
            await _applicationStateRepository.GetAsync(x => x.Name.Trim() == applicationState.Name.Trim()) is not null;

        if (isExistName)
            await throwBusinessException(ApplicationStatesBusinessMessages.ApplicationStateExists);
    }

    public async Task ApplicationStateShouldNotExistWhenUpdate(ApplicationState applicationState)
    {
        bool isExistName = false;

        if (applicationState.Name is not null)
            isExistName =
                await _applicationStateRepository.GetAsync(x =>
                    x.Id != applicationState.Id && x.Name.Trim() == applicationState.Name.Trim()
                )
                    is not null;

        if (isExistName)
            await throwBusinessException(ApplicationStatesBusinessMessages.ApplicationStateExists);
    }
}
