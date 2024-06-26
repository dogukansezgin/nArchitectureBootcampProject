using Application.Features.Applications.Commands.Create;
using Application.Features.Applications.Commands.Delete;
using Application.Features.Applications.Commands.Restore;
using Application.Features.Applications.Commands.Update;
using Application.Features.Applications.Queries.AppliedBootcamps;
using Application.Features.Applications.Queries.CheckApplication;
using Application.Features.Applications.Queries.GetById;
using Application.Features.Applications.Queries.GetList;
using Application.Features.Applications.Queries.GetListByInstructor;
using Application.Features.Applications.Queries.GetListByInstructorByState;
using Application.Features.Applications.Queries.GetListByInstructorDeleted;
using Application.Features.Applications.Queries.GetListByJoin;
using Application.Features.Applications.Queries.GetListDeleted;
using AutoMapper;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using ApplicationEntity = Domain.Entities.Application;

namespace Application.Features.Applications.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<ApplicationEntity, CreateApplicationCommand>().ReverseMap();
        CreateMap<ApplicationEntity, CreatedApplicationResponse>().ReverseMap();
        CreateMap<ApplicationEntity, UpdateApplicationCommand>().ReverseMap();
        CreateMap<ApplicationEntity, UpdatedApplicationResponse>().ReverseMap();
        CreateMap<ApplicationEntity, DeleteApplicationCommand>().ReverseMap();
        CreateMap<ApplicationEntity, DeletedApplicationResponse>().ReverseMap();
        CreateMap<ApplicationEntity, GetByIdApplicationResponse>().ReverseMap();
        CreateMap<ApplicationEntity, GetListApplicationListItemDto>().ReverseMap();
        CreateMap<IPaginate<ApplicationEntity>, GetListResponse<GetListApplicationListItemDto>>().ReverseMap();

        CreateMap<ApplicationEntity, RestoreApplicationCommand>().ReverseMap();
        CreateMap<ApplicationEntity, RestoredApplicationResponse>().ReverseMap();

        CreateMap<ApplicationEntity, GetListDeletedApplicationListItemDto>().ReverseMap();
        CreateMap<IPaginate<ApplicationEntity>, GetListResponse<GetListDeletedApplicationListItemDto>>().ReverseMap();

        CreateMap<ApplicationEntity, CheckApplicationResponse>().ReverseMap();
        CreateMap<ApplicationEntity, AppliedBootcampsResponse>().ReverseMap();
        CreateMap<IPaginate<ApplicationEntity>, GetListResponse<AppliedBootcampsResponse>>().ReverseMap();

        CreateMap<ApplicationEntity, GetListByInstructorApplicationListItemDto>().ReverseMap();
        CreateMap<IPaginate<ApplicationEntity>, GetListResponse<GetListByInstructorApplicationListItemDto>>().ReverseMap();

        CreateMap<ApplicationEntity, GetListByInstructorByStateApplicationListItemDto>().ReverseMap();
        CreateMap<IPaginate<ApplicationEntity>, GetListResponse<GetListByInstructorByStateApplicationListItemDto>>().ReverseMap();

        CreateMap<ApplicationEntity, GetListByInstructorDeletedApplicationListItemDto>().ReverseMap();
        CreateMap<IPaginate<ApplicationEntity>, GetListResponse<GetListByInstructorDeletedApplicationListItemDto>>().ReverseMap();

        CreateMap<ApplicationEntity, GetListByJoinApplicationListItemDto>().ReverseMap();
        CreateMap<IPaginate<ApplicationEntity>, GetListResponse<GetListByJoinApplicationListItemDto>>().ReverseMap();
    }
}
