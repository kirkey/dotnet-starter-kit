using System.Collections.ObjectModel;

namespace Shared.Authorization;

public static class FshRoles
{
    public const string Admin = nameof(Admin);
    public const string Basic = nameof(Basic);

    public static IReadOnlyList<string> DefaultRoles { get; } = new ReadOnlyCollection<string>([
        Admin,
        Basic
    ]);

    public static bool IsDefault(string roleName) => DefaultRoles.Any(r => r == roleName);
}
