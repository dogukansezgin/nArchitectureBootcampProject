using System.Linq.Expressions;
using Application.Features.InstructorImages.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Services.InstructorImages;

public class InstructorImageManager : IInstructorImageService
{
    private readonly IInstructorImageRepository _instructorImageRepository;
    private readonly InstructorImageBusinessRules _instructorImageBusinessRules;

    public InstructorImageManager(
        IInstructorImageRepository instructorImageRepository,
        InstructorImageBusinessRules instructorImageBusinessRules
    )
    {
        _instructorImageRepository = instructorImageRepository;
        _instructorImageBusinessRules = instructorImageBusinessRules;
    }

    public async Task<InstructorImage?> GetAsync(
        Expression<Func<InstructorImage, bool>> predicate,
        Func<IQueryable<InstructorImage>, IIncludableQueryable<InstructorImage, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        InstructorImage? instructorImage = await _instructorImageRepository.GetAsync(
            predicate,
            include,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return instructorImage;
    }

    public async Task<IPaginate<InstructorImage>?> GetListAsync(
        Expression<Func<InstructorImage, bool>>? predicate = null,
        Func<IQueryable<InstructorImage>, IOrderedQueryable<InstructorImage>>? orderBy = null,
        Func<IQueryable<InstructorImage>, IIncludableQueryable<InstructorImage, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<InstructorImage> instructorImageList = await _instructorImageRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return instructorImageList;
    }

    public async Task<InstructorImage> AddAsync(InstructorImage instructorImage)
    {
        InstructorImage addedInstructorImage = await _instructorImageRepository.AddAsync(instructorImage);

        return addedInstructorImage;
    }

    public async Task<InstructorImage> UpdateAsync(InstructorImage instructorImage)
    {
        InstructorImage updatedInstructorImage = await _instructorImageRepository.UpdateAsync(instructorImage);

        return updatedInstructorImage;
    }

    public async Task<InstructorImage> DeleteAsync(InstructorImage instructorImage, bool permanent = false)
    {
        InstructorImage deletedInstructorImage = await _instructorImageRepository.DeleteAsync(instructorImage);

        return deletedInstructorImage;
    }
}
