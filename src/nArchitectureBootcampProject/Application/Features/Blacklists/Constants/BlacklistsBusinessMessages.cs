namespace Application.Features.Blacklists.Constants;

public static class BlacklistsBusinessMessages
{
    public const string SectionName = "Blacklist";

    public const string BlacklistNotExists = "Blacklist is not exists.";
    public const string BlacklistExists = "Blacklist is already exists.";
    public const string ApplicantBlacklisted = "This user could not be added because it is blacklisted.";
}
