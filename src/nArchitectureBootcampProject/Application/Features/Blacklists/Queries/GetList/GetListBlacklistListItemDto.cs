using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Blacklists.Queries.GetList;

public class GetListBlacklistListItemDto : IDto
{
    public Guid Id { get; set; }

    public Guid ApplicantId { get; set; }
    public string ApplicantUserName { get; set; }
    public string ApplicantEmail { get; set; }

    public string Reason { get; set; }
    public DateTime Date { get; set; }
    public DateTime CreatedDate { get; set; }
}
