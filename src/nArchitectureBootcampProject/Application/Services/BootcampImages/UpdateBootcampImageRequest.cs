namespace Application.Services.BootcampImages;

public class UpdateBootcampImageRequest
{
    public Guid Id { get; set; }
    public Guid BootcampId { get; set; }
}
