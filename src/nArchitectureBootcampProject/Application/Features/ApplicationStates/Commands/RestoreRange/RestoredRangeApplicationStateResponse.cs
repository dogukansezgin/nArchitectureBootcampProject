using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.ApplicationStates.Commands.RestoreRange;

public class RestoredRangeApplicationStateResponse : IResponse
{
    public ICollection<Guid> Ids { get; set; }
}
