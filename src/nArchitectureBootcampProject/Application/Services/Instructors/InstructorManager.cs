using System.Linq.Expressions;
using Application.Features.Applicants.Rules;
using Application.Features.Bootcamps.Rules;
using Application.Features.Instructors.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Services.Instructors;

public class InstructorManager : IInstructorService
{
    private readonly IInstructorRepository _instructorRepository;
    private readonly InstructorBusinessRules _instructorBusinessRules;

    public InstructorManager(IInstructorRepository instructorRepository, InstructorBusinessRules instructorBusinessRules)
    {
        _instructorRepository = instructorRepository;
        _instructorBusinessRules = instructorBusinessRules;
    }

    public async Task<Instructor?> GetAsync(
        Expression<Func<Instructor, bool>> predicate,
        Func<IQueryable<Instructor>, IIncludableQueryable<Instructor, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        Instructor? instructor = await _instructorRepository.GetAsync(
            predicate,
            include,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return instructor;
    }

    public async Task<IPaginate<Instructor>?> GetListAsync(
        Expression<Func<Instructor, bool>>? predicate = null,
        Func<IQueryable<Instructor>, IOrderedQueryable<Instructor>>? orderBy = null,
        Func<IQueryable<Instructor>, IIncludableQueryable<Instructor, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<Instructor> instructorList = await _instructorRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return instructorList;
    }

    public async Task<Instructor> AddAsync(Instructor instructor)
    {
        await _instructorBusinessRules.InstructorShouldNotExist(instructor);

        Instructor addedInstructor = await _instructorRepository.AddAsync(instructor);

        return addedInstructor;
    }

    public async Task<Instructor> UpdateAsync(Instructor instructor)
    {
        await _instructorBusinessRules.InstructorShouldExistWhenSelected(instructor);
        await _instructorBusinessRules.InstructorIdShouldExistWhenSelected(instructor.Id);

        Instructor updatedInstructor = await _instructorRepository.UpdateAsync(instructor);

        return updatedInstructor;
    }

    public async Task<Instructor> DeleteAsync(Instructor instructor, bool permanent = false)
    {
        await _instructorBusinessRules.InstructorShouldExistWhenSelected(instructor);

        Instructor deletedInstructor = await _instructorRepository.DeleteAsync(instructor, permanent);

        return deletedInstructor;
    }

    public async Task<ICollection<Instructor>> DeleteRangeAsync(ICollection<Instructor> instructors, bool permanent = false)
    {
        foreach (Instructor instructor in instructors)
        {
            await _instructorBusinessRules.InstructorShouldExistWhenSelected(instructor);
        }

        ICollection<Instructor> deletedInstructors = await _instructorRepository.DeleteRangeCustomAsync(instructors, permanent);

        return deletedInstructors;
    }

    public async Task<Instructor> RestoreAsync(Instructor instructor)
    {
        await _instructorBusinessRules.InstructorShouldExistWhenSelected(instructor);

        Instructor restoredInstructor = await _instructorRepository.RestoreAsync(instructor);

        return restoredInstructor;
    }

    public async Task<ICollection<Instructor>> RestoreRangeAsync(ICollection<Instructor> instructors)
    {
        foreach (Instructor instructor in instructors)
        {
            await _instructorBusinessRules.InstructorShouldExistWhenSelected(instructor);
        }

        ICollection<Instructor> deletedInstructors = await _instructorRepository.RestoreRangeCustomAsync(instructors);

        return deletedInstructors;
    }

    public async Task<Instructor> GetByIdAsync(Guid id)
    {
        Instructor? instructor = await GetAsync(x => x.Id == id);

        await _instructorBusinessRules.InstructorShouldExistWhenSelected(instructor);

        return instructor;
    }
}
