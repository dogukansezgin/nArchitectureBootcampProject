using Application.Features.Bootcamps.Constants;
using Application.Features.BootcampStates.Rules;
using Application.Features.Instructors.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.Bootcamps.Rules;

public class BootcampBusinessRules : BaseBusinessRules
{
    private readonly IBootcampRepository _bootcampRepository;
    private readonly ILocalizationService _localizationService;
    private readonly InstructorBusinessRules _instructorBusinessRules;
    private readonly BootcampStateBusinessRules _bootcampStateBusinessRules;

    public BootcampBusinessRules(
        IBootcampRepository bootcampRepository,
        ILocalizationService localizationService,
        InstructorBusinessRules instructorBusinessRules,
        BootcampStateBusinessRules bootcampStateBusinessRules
    )
    {
        _bootcampRepository = bootcampRepository;
        _localizationService = localizationService;
        _instructorBusinessRules = instructorBusinessRules;
        _bootcampStateBusinessRules = bootcampStateBusinessRules;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, BootcampsBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task BootcampShouldExistWhenSelected(Bootcamp? bootcamp)
    {
        if (bootcamp == null)
            await throwBusinessException(BootcampsBusinessMessages.BootcampNotExists);
    }

    public async Task BootcampIdShouldExistWhenSelected(Guid id)
    {
        Bootcamp? bootcamp = await _bootcampRepository.GetAsync(predicate: b => b.Id == id, enableTracking: false);
        await BootcampShouldExistWhenSelected(bootcamp);
    }

    public async Task BootcampForeignKeysShouldExist(Bootcamp? bootcamp)
    {
        await BootcampShouldExistWhenSelected(bootcamp);

        await _instructorBusinessRules.InstructorIdShouldExistWhenSelected(bootcamp.InstructorId);
        await _bootcampStateBusinessRules.BootcampStateIdShouldExistWhenSelected(bootcamp.BootcampStateId);
    }

    public async Task BootcampShouldNotExist(Bootcamp? bootcamp)
    {
        var isExistName = _bootcampRepository.Get(x => x.Name.Trim() == bootcamp.Name.Trim()) is not null;

        if (isExistName)
            await throwBusinessException(BootcampsBusinessMessages.BootcampExists);
    }
}
