using Application.Features.Applicants.Commands.Create;
using Application.Features.Applicants.Commands.Delete;
using Application.Features.Applicants.Commands.Restore;
using Application.Features.Applicants.Commands.Update;
using Application.Features.Applicants.Commands.UpdateInfoFromAuth;
using Application.Features.Applicants.Queries.GetBasicInfoList;
using Application.Features.Applicants.Queries.GetById;
using Application.Features.Applicants.Queries.GetList;
using Application.Features.Applicants.Queries.GetListDeleted;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Applicants.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Applicant, CreateApplicantCommand>().ReverseMap();
        CreateMap<Applicant, CreatedApplicantResponse>().ReverseMap();
        CreateMap<Applicant, UpdateApplicantCommand>().ReverseMap();
        CreateMap<Applicant, UpdatedApplicantResponse>().ReverseMap();
        CreateMap<Applicant, DeleteApplicantCommand>().ReverseMap();
        CreateMap<Applicant, DeletedApplicantResponse>().ReverseMap();
        CreateMap<Applicant, GetByIdApplicantResponse>().ReverseMap();
        CreateMap<Applicant, GetListApplicantListItemDto>().ReverseMap();
        CreateMap<IPaginate<Applicant>, GetListResponse<GetListApplicantListItemDto>>().ReverseMap();

        CreateMap<Applicant, GetListDeletedApplicantListItemDto>().ReverseMap();
        CreateMap<IPaginate<Applicant>, GetListResponse<GetListDeletedApplicantListItemDto>>().ReverseMap();

        CreateMap<Applicant, GetBasicInfoApplicantListItemDto>().ReverseMap();
        CreateMap<IPaginate<Applicant>, GetListResponse<GetBasicInfoApplicantListItemDto>>().ReverseMap();

        CreateMap<Applicant, RestoreApplicantCommand>().ReverseMap();
        CreateMap<Applicant, RestoredApplicantResponse>().ReverseMap();

        CreateMap<Applicant, UpdateApplicantInfoFromAuthCommand>()
            .ReverseMap()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Applicant, UpdatedApplicantInfoFromAuthResponse>().ReverseMap();
    }
}
