namespace FSH.Framework.Core.Identity.Roles.Features.UpdatePermissions;
public class UpdatePermissionsCommand
{
    public string RoleId { get; set; } = null!;
    public List<string> Permissions { get; set; } = null!;
}
