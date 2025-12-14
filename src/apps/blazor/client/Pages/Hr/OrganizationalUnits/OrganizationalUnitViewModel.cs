namespace FSH.Starter.Blazor.Client.Pages.Hr.OrganizationalUnits;

/// <summary>
/// View model for organizational unit CRUD operations.
/// Extends the update command with additional display properties.
/// </summary>
public class OrganizationalUnitViewModel : UpdateOrganizationalUnitCommand
{
    /// <summary>
    /// The unique code for the organizational unit.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// The type of organizational unit (Department, Division, Section).
    /// </summary>
    public OrganizationalUnitType Type { get; set; }

    /// <summary>
    /// The hierarchy level (1=Department, 2=Division, 3=Section).
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// The parent organizational unit ID.
    /// </summary>
    public new DefaultIdType? ParentId { get; set; }

    /// <summary>
    /// The name of the parent organizational unit.
    /// </summary>
    public string? ParentName { get; set; }

    /// <summary>
    /// The name of the manager/head of this unit.
    /// </summary>
    public string? ManagerName { get; set; }

    /// <summary>
    /// The full hierarchy path (e.g., "/HR-001/DIV-001/").
    /// </summary>
    public string? HierarchyPath { get; set; }

    /// <summary>
    /// Whether the organizational unit is active and available for assignments.
    /// </summary>
    public bool IsActive { get; set; }
}

