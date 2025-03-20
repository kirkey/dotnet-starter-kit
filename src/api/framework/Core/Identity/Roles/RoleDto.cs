namespace FSH.Framework.Core.Identity.Roles;

public class RoleDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public List<string>? Permissions { get; set; }
}
