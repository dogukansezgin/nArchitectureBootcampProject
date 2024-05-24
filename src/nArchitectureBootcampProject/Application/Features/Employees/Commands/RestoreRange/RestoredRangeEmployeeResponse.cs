using NArchitecture.Core.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Employees.Commands.RestoreRange;
public class RestoredRangeEmployeeResponse : IResponse
{
    public ICollection<Guid> Ids { get; set; }
}
