namespace Application.Features.Applications.Queries.AppliedBootcamps;
public class AppliedBootcampsResponse
{
    public Guid Id { get; set; }
    public Guid ApplicantId { get; set; }
    public Guid ApplicationStateId { get; set; }

    public Guid BootcampId { get; set; }
    public string BootcampName { get; set; }

    public Guid BootcampInstructorId { get; set; }
    public string BootcampInstructorUserName { get; set; }
    public string? BootcampInstructorFirstName { get; set; }
    public string? BootcampInstructorLastName { get; set; }
    public string BootcampInstructorCompanyName { get; set; }

    public Guid BootcampBootcampStateId { get; set; }
    public string BootcampBootcampStateName { get; set; }

    public DateTime BootcampStartDate { get; set; }
    public DateTime BootcampEndDate { get; set; }

}
