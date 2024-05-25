using Application.Features.BootcampStates.Constants;
using Application.Features.BootcampStates.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.BootcampStates.Rules;

public class BootcampStateBusinessRules : BaseBusinessRules
{
    private readonly IBootcampStateRepository _bootcampStateRepository;
    private readonly ILocalizationService _localizationService;

    public BootcampStateBusinessRules(IBootcampStateRepository bootcampStateRepository, ILocalizationService localizationService)
    {
        _bootcampStateRepository = bootcampStateRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, BootcampStatesBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task BootcampStateShouldExistWhenSelected(BootcampState? bootcampState)
    {
        if (bootcampState == null)
            await throwBusinessException(BootcampStatesBusinessMessages.BootcampStateNotExists);
    }

    public async Task BootcampStateIdShouldExistWhenSelected(Guid id)
    {
        BootcampState? bootcampState = await _bootcampStateRepository.GetAsync(
            predicate: bs => bs.Id == id,
            enableTracking: false
        );
        await BootcampStateShouldExistWhenSelected(bootcampState);
    }

    public async Task BootcampStateShouldNotExist(BootcampState bootcampState)
    {
        var isExistName =
            await _bootcampStateRepository.GetAsync(x => x.Name.Trim() == bootcampState.Name.Trim()) is not null;

        if (isExistName)
            await throwBusinessException(BootcampStatesBusinessMessages.BootcampStateExists);
    }

    public async Task BootcampStateShouldNotExistWhenUpdate(BootcampState bootcampState)
    {
        bool isExistName = false;

        if (bootcampState.Name is not null)
            isExistName =
                await _bootcampStateRepository.GetAsync(x => x.Id != bootcampState.Id && x.Name.Trim() == bootcampState.Name.Trim())
                    is not null;

        if (isExistName)
            await throwBusinessException(BootcampStatesBusinessMessages.BootcampStateExists);
    }
}
