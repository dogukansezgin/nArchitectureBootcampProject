using System.Linq.Expressions;
using Application.Features.BootcampImages.Rules;
using Application.Services.ImageService;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Services.BootcampImages;

public class BootcampImageManager : IBootcampImageService
{
    private readonly IBootcampImageRepository _bootcampImageRepository;
    private readonly BootcampImageBusinessRules _bootcampImageBusinessRules;
    private readonly ImageServiceBase _imageService;
    private readonly IMapper _mapper;

    public BootcampImageManager(
        IBootcampImageRepository bootcampImageRepository,
        BootcampImageBusinessRules bootcampImageBusinessRules,
        ImageServiceBase imageService,
        IMapper mapper
    )
    {
        _bootcampImageRepository = bootcampImageRepository;
        _bootcampImageBusinessRules = bootcampImageBusinessRules;
        _imageService = imageService;
        _mapper = mapper;
    }

    public async Task<BootcampImage?> GetAsync(
        Expression<Func<BootcampImage, bool>> predicate,
        Func<IQueryable<BootcampImage>, IIncludableQueryable<BootcampImage, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        BootcampImage? bootcampImage = await _bootcampImageRepository.GetAsync(
            predicate,
            include,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return bootcampImage;
    }

    public async Task<IPaginate<BootcampImage>?> GetListAsync(
        Expression<Func<BootcampImage, bool>>? predicate = null,
        Func<IQueryable<BootcampImage>, IOrderedQueryable<BootcampImage>>? orderBy = null,
        Func<IQueryable<BootcampImage>, IIncludableQueryable<BootcampImage, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<BootcampImage> bootcampImageList = await _bootcampImageRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return bootcampImageList;
    }

    public async Task<BootcampImage> AddAsync(IFormFile file, BootcampImageRequest request)
    {
        BootcampImage addedBootcampImage = new BootcampImage() { BootcampId = request.BootcampId };
        addedBootcampImage = _mapper.Map(request, addedBootcampImage);
        addedBootcampImage.ImagePath = await _imageService.UploadAsync(file);

        return await _bootcampImageRepository.AddAsync(addedBootcampImage);
    }

    public async Task<BootcampImage> UpdateAsync(IFormFile file, UpdateBootcampImageRequest request)
    {
        BootcampImage updatedBootcampImage = await _bootcampImageRepository.GetAsync(x => x.Id == request.Id);
        //updatedBootcampImage

        return updatedBootcampImage;
    }

    public async Task<BootcampImage> DeleteAsync(BootcampImage bootcampImage, bool permanent = false)
    {
        BootcampImage deletedBootcampImage = await _bootcampImageRepository.DeleteAsync(bootcampImage);

        return deletedBootcampImage;
    }
}
