﻿using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Instructors.Queries.GetBasicInfoList;

public class GetBasicInfoInstructorListItemDto : IDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string CompanyName { get; set; }
}
