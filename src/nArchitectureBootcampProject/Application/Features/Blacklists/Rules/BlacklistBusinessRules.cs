using Application.Features.Applicants.Rules;
using Application.Features.Blacklists.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.Blacklists.Rules;

public class BlacklistBusinessRules : BaseBusinessRules
{
    private readonly IBlacklistRepository _blacklistRepository;
    private readonly ILocalizationService _localizationService;
    private readonly ApplicantBusinessRules _applicantBusinessRules;

    public BlacklistBusinessRules(
        IBlacklistRepository blacklistRepository,
        ILocalizationService localizationService,
        ApplicantBusinessRules applicantBusinessRules
    )
    {
        _blacklistRepository = blacklistRepository;
        _localizationService = localizationService;
        _applicantBusinessRules = applicantBusinessRules;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, BlacklistsBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task BlacklistShouldExistWhenSelected(Blacklist? blacklist)
    {
        if (blacklist == null)
            await throwBusinessException(BlacklistsBusinessMessages.BlacklistNotExists);
    }

    public async Task BlacklistIdShouldExistWhenSelected(Guid id)
    {
        Blacklist? blacklist = await _blacklistRepository.GetAsync(predicate: b => b.Id == id, enableTracking: false);
        await BlacklistShouldExistWhenSelected(blacklist);
    }

    public async Task BlacklistForeignKeysShouldExist(Blacklist? blacklist)
    {
        await BlacklistShouldExistWhenSelected(blacklist);

        await _applicantBusinessRules.ApplicantIdShouldExistWhenSelected(blacklist.ApplicantId);
    }

    public async Task BlacklistApplicantCheck(Guid applicantId)
    {
        Blacklist? blacklist = await _blacklistRepository.GetAsync(x => x.ApplicantId == applicantId);

        if (blacklist != null)
            await throwBusinessException(BlacklistsBusinessMessages.ApplicantBlacklisted);
    }

    public async Task BlacklistApplicantCheckUpdate(Blacklist blacklist)
    {
        var isExist = await _blacklistRepository.GetAsync(x => x.ApplicantId == blacklist.ApplicantId && x.Id != blacklist.Id);

        if (isExist != null)
            await throwBusinessException(BlacklistsBusinessMessages.ApplicantBlacklisted);
    }
}
