using NArchitecture.Core.Application.Responses;

namespace Application.Features.Employees.Commands.Restore;
public class RestoredEmployeeResponse : IResponse
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
}