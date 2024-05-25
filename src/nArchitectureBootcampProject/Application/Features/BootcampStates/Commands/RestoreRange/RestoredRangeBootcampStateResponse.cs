using NArchitecture.Core.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.BootcampStates.Commands.RestoreRange;
public class RestoredRangeBootcampStateResponse : IResponse
{
    public ICollection<Guid> Ids { get; set; }
}
