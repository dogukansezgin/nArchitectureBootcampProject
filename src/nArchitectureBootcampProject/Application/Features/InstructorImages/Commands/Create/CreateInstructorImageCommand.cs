using Application.Features.Employees.Constants;
using Application.Features.InstructorImages.Constants;
using Application.Features.InstructorImages.Rules;
using Application.Features.Instructors.Constants;
using Application.Features.Users.Constants;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.InstructorImages.Constants.InstructorImagesOperationClaims;

namespace Application.Features.InstructorImages.Commands.Create;

public class CreateInstructorImageCommand : IRequest<CreatedInstructorImageResponse>, ISecuredRequest
//ICacheRemoverRequest,
//ILoggableRequest,
//ITransactionalRequest
{
    public Guid InstructorId { get; set; }
    public string ImagePath { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User, InstructorsOperationClaims.User];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetInstructorImages"];

    public class CreateInstructorImageCommandHandler
        : IRequestHandler<CreateInstructorImageCommand, CreatedInstructorImageResponse>
    {
        private readonly IMapper _mapper;
        private readonly IInstructorImageRepository _instructorImageRepository;
        private readonly InstructorImageBusinessRules _instructorImageBusinessRules;

        public CreateInstructorImageCommandHandler(
            IMapper mapper,
            IInstructorImageRepository instructorImageRepository,
            InstructorImageBusinessRules instructorImageBusinessRules
        )
        {
            _mapper = mapper;
            _instructorImageRepository = instructorImageRepository;
            _instructorImageBusinessRules = instructorImageBusinessRules;
        }

        public async Task<CreatedInstructorImageResponse> Handle(
            CreateInstructorImageCommand request,
            CancellationToken cancellationToken
        )
        {
            InstructorImage instructorImage = _mapper.Map<InstructorImage>(request);

            await _instructorImageRepository.AddAsync(instructorImage);

            CreatedInstructorImageResponse response = _mapper.Map<CreatedInstructorImageResponse>(instructorImage);
            return response;
        }
    }
}
