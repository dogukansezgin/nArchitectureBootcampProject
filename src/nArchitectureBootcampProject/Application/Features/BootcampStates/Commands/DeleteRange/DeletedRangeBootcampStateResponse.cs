﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.BootcampStates.Commands.DeleteRange;

public class DeletedRangeBootcampStateResponse : IResponse
{
    public ICollection<Guid> Ids { get; set; }
    public DateTime DeletedDate { get; set; }
    public bool IsPermament { get; set; }
}
