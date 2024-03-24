using Application.Features.InstructorImages.Constants;
using Application.Features.InstructorImages.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using MediatR;
using static Application.Features.InstructorImages.Constants.InstructorImagesOperationClaims;

namespace Application.Features.InstructorImages.Queries.GetById;

public class GetByIdInstructorImageQuery : IRequest<GetByIdInstructorImageResponse>, ISecuredRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [Admin, Read];

    public class GetByIdInstructorImageQueryHandler : IRequestHandler<GetByIdInstructorImageQuery, GetByIdInstructorImageResponse>
    {
        private readonly IMapper _mapper;
        private readonly IInstructorImageRepository _instructorImageRepository;
        private readonly InstructorImageBusinessRules _instructorImageBusinessRules;

        public GetByIdInstructorImageQueryHandler(IMapper mapper, IInstructorImageRepository instructorImageRepository, InstructorImageBusinessRules instructorImageBusinessRules)
        {
            _mapper = mapper;
            _instructorImageRepository = instructorImageRepository;
            _instructorImageBusinessRules = instructorImageBusinessRules;
        }

        public async Task<GetByIdInstructorImageResponse> Handle(GetByIdInstructorImageQuery request, CancellationToken cancellationToken)
        {
            InstructorImage? instructorImage = await _instructorImageRepository.GetAsync(predicate: ii => ii.Id == request.Id, cancellationToken: cancellationToken);
            await _instructorImageBusinessRules.InstructorImageShouldExistWhenSelected(instructorImage);

            GetByIdInstructorImageResponse response = _mapper.Map<GetByIdInstructorImageResponse>(instructorImage);
            return response;
        }
    }
}