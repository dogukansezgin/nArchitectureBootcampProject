using Application.Features.Applicants.Constants;
using Application.Features.Applicants.Rules;
using Application.Services.Applicants;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using static Application.Features.Applicants.Constants.ApplicantsOperationClaims;

namespace Application.Features.Applicants.Queries.GetById;

public class GetByIdApplicantQuery : IRequest<GetByIdApplicantResponse>, ISecuredRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [Admin, Read];

    public class GetByIdApplicantQueryHandler : IRequestHandler<GetByIdApplicantQuery, GetByIdApplicantResponse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicantService _applicantService;

        public GetByIdApplicantQueryHandler(IMapper mapper, IApplicantService applicantService)
        {
            _mapper = mapper;
            _applicantService = applicantService;
        }

        public async Task<GetByIdApplicantResponse> Handle(GetByIdApplicantQuery request, CancellationToken cancellationToken)
        {
            Applicant? applicant = await _applicantService.GetByIdAsync(request.Id);

            GetByIdApplicantResponse response = _mapper.Map<GetByIdApplicantResponse>(applicant);
            return response;
        }
    }
}
