using Application.Features.Employees.Constants;
using Application.Features.InstructorImages.Constants;
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

namespace Application.Features.InstructorImages.Commands.Delete;

public class DeleteInstructorImageCommand : IRequest<DeletedInstructorImageResponse>, ISecuredRequest
//ICacheRemoverRequest,
//ILoggableRequest,
//ITransactionalRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [UsersOperationClaims.Admin, EmployeesOperationClaims.User, InstructorsOperationClaims.User];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetInstructorImages"];

    public class DeleteInstructorImageCommandHandler
        : IRequestHandler<DeleteInstructorImageCommand, DeletedInstructorImageResponse>
    {
        private readonly IMapper _mapper;
        private readonly IInstructorImageRepository _instructorImageRepository;
        private readonly InstructorImageBusinessRules _instructorImageBusinessRules;

        public DeleteInstructorImageCommandHandler(
            IMapper mapper,
            IInstructorImageRepository instructorImageRepository,
            InstructorImageBusinessRules instructorImageBusinessRules
        )
        {
            _mapper = mapper;
            _instructorImageRepository = instructorImageRepository;
            _instructorImageBusinessRules = instructorImageBusinessRules;
        }

        public async Task<DeletedInstructorImageResponse> Handle(
            DeleteInstructorImageCommand request,
            CancellationToken cancellationToken
        )
        {
            InstructorImage? instructorImage = await _instructorImageRepository.GetAsync(
                predicate: ii => ii.Id == request.Id,
                cancellationToken: cancellationToken
            );
            await _instructorImageBusinessRules.InstructorImageShouldExistWhenSelected(instructorImage);

            await _instructorImageRepository.DeleteAsync(instructorImage!);

            DeletedInstructorImageResponse response = _mapper.Map<DeletedInstructorImageResponse>(instructorImage);
            return response;
        }
    }
}
