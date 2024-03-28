using Application.Features.Applicants.Constants;
using Application.Services.Applicants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.Applicants.Rules;

public class ApplicantBusinessRules : BaseBusinessRules
{
    private readonly ILocalizationService _localizationService;
    private readonly IApplicantRepository _applicantRepository;

    public ApplicantBusinessRules(ILocalizationService localizationService, IApplicantRepository applicantRepository)
    {
        _localizationService = localizationService;
        _applicantRepository = applicantRepository;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, ApplicantsBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task ApplicantShouldExistWhenSelected(Applicant? applicant)
    {
        if (applicant == null)
            await throwBusinessException(ApplicantsBusinessMessages.ApplicantNotExists);
    }

    public async Task ApplicantIdShouldExistWhenSelected(Guid id)
    {
        Applicant? applicant = await _applicantRepository.GetAsync(
            predicate: a => a.Id == id,
            enableTracking: false
        );
        await ApplicantShouldExistWhenSelected(applicant);
    }

    public async Task ApplicantShouldExist(Guid id)
    {
        Applicant? applicant = await _applicantRepository.GetAsync(x => x.Id == id);
        if (applicant == null) await throwBusinessException(ApplicantsBusinessMessages.ApplicantNotExists);
    }

    public async Task ApplicantShouldNotExist(Applicant applicant)
    {
        var isExistId = await _applicantRepository.GetAsync(x => x.Id == applicant.Id) is not null;
        var isExistUserName = await _applicantRepository.GetAsync(x => x.UserName.Trim() == applicant.UserName.Trim()) is not null;
        var isExistNationalId = await _applicantRepository.GetAsync(x => x.NationalIdentity.Trim() == applicant.NationalIdentity.Trim()) is not null;
        var isExistEmail = await _applicantRepository.GetAsync(x => x.Email.Trim() == applicant.Email.Trim()) is not null;

        if (isExistId || isExistUserName || isExistNationalId || isExistEmail) await throwBusinessException(ApplicantsBusinessMessages.ApplicantExists);
    }

}
