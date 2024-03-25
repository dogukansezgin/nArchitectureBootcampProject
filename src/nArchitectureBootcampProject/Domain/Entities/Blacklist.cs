using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class Blacklist : Entity<Guid>
{
    public Guid ApplicantId { get; set; }
    public string Reason { get; set; }
    public DateTime Date { get; set; }

    public virtual Applicant? Applicant { get; set; }

    public Blacklist() { }

    public Blacklist(Guid id, Guid applicantId, string reason, DateTime date)
    {
        Id = id;
        ApplicantId = applicantId;
        Reason = reason;
        Date = date;
    }
}
