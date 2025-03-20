namespace FSH.Framework.Core.Identity.Roles.Features.CreateOrUpdateRole;

public class CreateOrUpdateRoleCommand
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
