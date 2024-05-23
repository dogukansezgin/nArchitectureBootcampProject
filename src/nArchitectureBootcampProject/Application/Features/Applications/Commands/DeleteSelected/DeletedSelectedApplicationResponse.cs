using NArchitecture.Core.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Applications.Commands.DeleteSelected;
public class DeletedSelectedApplicationResponse : IResponse
{
    public ICollection<Guid> Ids { get; set; }
    public DateTime DeletedDate { get; set; }
}
