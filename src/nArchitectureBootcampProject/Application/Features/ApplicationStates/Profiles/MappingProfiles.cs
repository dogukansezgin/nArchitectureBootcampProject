using Application.Features.ApplicationStates.Commands.Create;
using Application.Features.ApplicationStates.Commands.Delete;
using Application.Features.ApplicationStates.Commands.Restore;
using Application.Features.ApplicationStates.Commands.Update;
using Application.Features.ApplicationStates.Queries.GetById;
using Application.Features.ApplicationStates.Queries.GetByName;
using Application.Features.ApplicationStates.Queries.GetList;
using Application.Features.ApplicationStates.Queries.GetListDeleted;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.ApplicationStates.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<ApplicationState, CreateApplicationStateCommand>().ReverseMap();
        CreateMap<ApplicationState, CreatedApplicationStateResponse>().ReverseMap();
        CreateMap<ApplicationState, UpdateApplicationStateCommand>().ReverseMap();
        CreateMap<ApplicationState, UpdatedApplicationStateResponse>().ReverseMap();
        CreateMap<ApplicationState, DeleteApplicationStateCommand>().ReverseMap();
        CreateMap<ApplicationState, DeletedApplicationStateResponse>().ReverseMap();
        CreateMap<ApplicationState, GetByIdApplicationStateResponse>().ReverseMap();
        CreateMap<ApplicationState, GetByNameApplicationStateResponse>().ReverseMap();
        CreateMap<ApplicationState, GetListApplicationStateListItemDto>().ReverseMap();
        CreateMap<IPaginate<ApplicationState>, GetListResponse<GetListApplicationStateListItemDto>>().ReverseMap();

        CreateMap<ApplicationState, RestoreApplicationStateCommand>().ReverseMap();
        CreateMap<ApplicationState, RestoredApplicationStateResponse>().ReverseMap();

        CreateMap<ApplicationState, GetListDeletedApplicationStateListItemDto>().ReverseMap();
        CreateMap<IPaginate<ApplicationState>, GetListResponse<GetListDeletedApplicationStateListItemDto>>().ReverseMap();
    }
}
