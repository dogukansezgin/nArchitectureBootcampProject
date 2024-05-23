using Application.Features.Applications.Commands.Create;
using Application.Features.Applications.Commands.Delete;
using Application.Features.Applications.Commands.Update;
using Application.Features.Applications.Queries.AppliedBootcamps;
using Application.Features.Applications.Queries.CheckApplication;
using Application.Features.Applications.Queries.GetById;
using Application.Features.Applications.Queries.GetList;
using Application.Features.Applications.Queries.GetListByState;
using Application.Features.Applications.Queries.GetListByUserName;
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
        CreateMap<IPaginate<ApplicationEntity>, GetListResponse<GetListByUserNameApplicationListItemDto>>().ReverseMap();

        CreateMap<ApplicationEntity, CheckApplicationResponse>().ReverseMap();
        CreateMap<ApplicationEntity, AppliedBootcampsResponse>().ReverseMap();
        CreateMap<IPaginate<ApplicationEntity>, GetListResponse<AppliedBootcampsResponse>>().ReverseMap();
        CreateMap<ApplicationEntity,GetListApplicationByStateListItemDto>().ReverseMap();
        CreateMap<IPaginate<ApplicationEntity>, GetListResponse<GetListApplicationByStateListItemDto>>().ReverseMap();

        CreateMap<ApplicationEntity, GetListByUserNameApplicationListItemDto>().ReverseMap();
    }
}
