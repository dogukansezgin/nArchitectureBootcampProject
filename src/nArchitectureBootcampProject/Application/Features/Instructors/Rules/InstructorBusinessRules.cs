using Application.Features.Instructors.Constants;
using Application.Features.Instructors.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.Instructors.Rules;

public class InstructorBusinessRules : BaseBusinessRules
{
    private readonly IInstructorRepository _instructorRepository;
    private readonly ILocalizationService _localizationService;

    public InstructorBusinessRules(IInstructorRepository instructorRepository, ILocalizationService localizationService)
    {
        _instructorRepository = instructorRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, InstructorsBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task InstructorShouldExistWhenSelected(Instructor? instructor)
    {
        if (instructor == null)
            await throwBusinessException(InstructorsBusinessMessages.InstructorNotExists);
    }

    public async Task InstructorIdShouldExistWhenSelected(Guid id)
    {
        Instructor? instructor = await _instructorRepository.GetAsync(predicate: i => i.Id == id, enableTracking: false);
        await InstructorShouldExistWhenSelected(instructor);
    }

    public async Task InstructorShouldNotExist(Instructor instructor)
    {
        var isExistId = await _instructorRepository.GetAsync(x => x.Id == instructor.Id) is not null;
        var isExistUserName =
            await _instructorRepository.GetAsync(x => x.UserName.Trim() == instructor.UserName.Trim()) is not null;

        bool isExistNationalId = false;
        if (instructor.NationalIdentity != null)
        {
            isExistNationalId =
                await _instructorRepository.GetAsync(x => x.NationalIdentity.Trim() == instructor.NationalIdentity.Trim())
                    is not null;
        }
        ;

        var isExistEmail = await _instructorRepository.GetAsync(x => x.Email.Trim() == instructor.Email.Trim()) is not null;

        if (isExistId || isExistUserName || isExistNationalId || isExistEmail)
            await throwBusinessException(InstructorsBusinessMessages.InstructorExists);
    }

    public async Task InstructorShouldNotExistWhenUpdate(Instructor instructor)
    {
        bool isExistUserName = false;
        bool isExistNationalId = false;
        bool isExistEmail = false;

        if (instructor.UserName is not null)
            isExistUserName =
                await _instructorRepository.GetAsync(x =>
                    x.Id != instructor.Id && x.UserName.Trim() == instructor.UserName.Trim()
                )
                    is not null;

        if (instructor.NationalIdentity is not null)
            isExistNationalId =
                await _instructorRepository.GetAsync(x =>
                    x.Id != instructor.Id && x.NationalIdentity.Trim() == instructor.NationalIdentity.Trim()
                )
                    is not null;

        if (instructor.Email is not null)
            isExistEmail =
                await _instructorRepository.GetAsync(x => x.Id != instructor.Id && x.Email.Trim() == instructor.Email.Trim())
                    is not null;

        if (isExistUserName || isExistNationalId || isExistEmail)
            await throwBusinessException(InstructorsBusinessMessages.InstructorExists);
    }
}
