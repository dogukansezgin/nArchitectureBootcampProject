using Application.Features.Applications.Constants;
using Application.Features.Bootcamps.Constants;
using NArchitecture.Core.Security.Attributes;

namespace Application.Features.Instructors.Constants;

[OperationClaimConstants]
public static class InstructorsOperationClaims
{
    private const string _section = "Instructors";

    public const string User = $"{_section}.User";

    // idle claims
    //public const string Admin = $"{_section}.Admin";
    //public const string Read = $"{_section}.Read";
    //public const string Write = $"{_section}.Write";
    //public const string Create = $"{_section}.Create";
    //public const string Update = $"{_section}.Update";
    //public const string Delete = $"{_section}.Delete";

    public static readonly string[] InitialRolesArray = { User };

    public static readonly string[] InitialRoles = InitialRolesArray;
}
