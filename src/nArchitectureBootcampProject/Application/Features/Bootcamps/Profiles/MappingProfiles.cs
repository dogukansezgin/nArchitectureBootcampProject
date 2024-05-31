using Application.Features.Bootcamps.Commands.Create;
using Application.Features.Bootcamps.Commands.Delete;
using Application.Features.Bootcamps.Commands.Restore;
using Application.Features.Bootcamps.Commands.Update;
using Application.Features.Bootcamps.Queries.GetBasicInfoList;
using Application.Features.Bootcamps.Queries.GetById;
using Application.Features.Bootcamps.Queries.GetList;
using Application.Features.Bootcamps.Queries.GetListByInstructor;
using Application.Features.Bootcamps.Queries.GetListDeleted;
using Application.Features.Bootcamps.Queries.GetListFinished;
using Application.Features.Bootcamps.Queries.GetListUnfinished;
using Application.Features.Bootcamps.Queries.SearchAll;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Bootcamps.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Bootcamp, CreateBootcampCommand>().ReverseMap();
        CreateMap<Bootcamp, CreatedBootcampResponse>().ReverseMap();
        CreateMap<Bootcamp, UpdateBootcampCommand>().ReverseMap();
        CreateMap<Bootcamp, UpdatedBootcampResponse>().ReverseMap();
        CreateMap<Bootcamp, DeleteBootcampCommand>().ReverseMap();
        CreateMap<Bootcamp, DeletedBootcampResponse>().ReverseMap();
        CreateMap<Bootcamp, GetByIdBootcampResponse>().ReverseMap();
        CreateMap<Bootcamp, GetListBootcampListItemDto>().ReverseMap();
        CreateMap<IPaginate<Bootcamp>, GetListResponse<GetListBootcampListItemDto>>().ReverseMap();

        CreateMap<Bootcamp, GetListFinishedBootcampListItemDto>()
            .ForMember(
                destinationMember: x => x.BootcampImageId,
                memberOptions: opt => opt.MapFrom(x => x.BootcampImages.FirstOrDefault().Id)
            )
            .ForMember(
                destinationMember: x => x.BootcampImagePath,
                memberOptions: opt => opt.MapFrom(x => x.BootcampImages.FirstOrDefault().ImagePath)
            );
        CreateMap<IPaginate<Bootcamp>, GetListResponse<GetListFinishedBootcampListItemDto>>().ReverseMap();

        CreateMap<Bootcamp, GetListUnfinishedBootcampListItemDto>()
            .ForMember(
                destinationMember: x => x.BootcampImageId,
                memberOptions: opt => opt.MapFrom(x => x.BootcampImages.FirstOrDefault().Id)
            )
            .ForMember(
                destinationMember: x => x.BootcampImagePath,
                memberOptions: opt => opt.MapFrom(x => x.BootcampImages.FirstOrDefault().ImagePath)
            );
        CreateMap<IPaginate<Bootcamp>, GetListResponse<GetListUnfinishedBootcampListItemDto>>().ReverseMap();

        CreateMap<Bootcamp, GetListDeletedBootcampListItemDto>().ReverseMap();
        CreateMap<IPaginate<Bootcamp>, GetListResponse<GetListDeletedBootcampListItemDto>>().ReverseMap();

        CreateMap<Bootcamp, GetListByInstructorBootcampListItemDto>().ReverseMap();
        CreateMap<IPaginate<Bootcamp>, GetListResponse<GetListByInstructorBootcampListItemDto>>().ReverseMap();

        CreateMap<Bootcamp, GetBasicInfoBootcampListItemDto>().ReverseMap();
        CreateMap<IPaginate<Bootcamp>, GetListResponse<GetBasicInfoBootcampListItemDto>>().ReverseMap();

        CreateMap<Bootcamp, SearchAllBootcampListItemDto>().ReverseMap();
        CreateMap<IPaginate<Bootcamp>, GetListResponse<SearchAllBootcampListItemDto>>().ReverseMap();

        CreateMap<Bootcamp, RestoreBootcampCommand>().ReverseMap();
        CreateMap<Bootcamp, RestoredBootcampResponse>().ReverseMap();
    }
}
