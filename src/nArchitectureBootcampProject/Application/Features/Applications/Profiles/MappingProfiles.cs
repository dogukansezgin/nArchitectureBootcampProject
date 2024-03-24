using Application.Features.Applications.Commands.Create;
using Application.Features.Applications.Commands.Delete;
using Application.Features.Applications.Commands.Update;
using Application.Features.Applications.Queries.GetById;
using Application.Features.Applications.Queries.GetList;
using AutoMapper;
using NArchitecture.Core.Application.Responses;
using ApplicationEntity = Domain.Entities.Application;
using NArchitecture.Core.Persistence.Paging;

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
    }
}