using FSH.Starter.WebApi.HumanResources.Domain.Events;

namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents a hierarchical organizational unit (Department, Division, or Section).
/// </summary>
/// <remarks>
/// Supports hierarchical structure:
/// - Department (Level 1, no parent) - Required top level
/// - Division (Level 2, parent = Department) - Optional middle level
/// - Section (Level 3, parent = Division) - Optional, must be under Division
/// 
/// Business rules:
/// - Code must be unique within company
/// - Departments have no parent
/// - Divisions must have Department parent
/// - Sections must have Division parent (cannot be directly under Department)
/// - A Division can have multiple Sections
/// - Cannot create circular references
/// - Manager must be an active employee
/// </remarks>
public class OrganizationalUnit : AuditableEntity, IAggregateRoot
{
    private OrganizationalUnit() { }

    private OrganizationalUnit(
        DefaultIdType id,
        string code,
        string name,
        OrganizationalUnitType type,
        DefaultIdType? parentId,
        int level)
    {
        Id = id;
        Code = code;
        Name = name;
        Type = type;
        ParentId = parentId;
        Level = level;
        IsActive = true;

        QueueDomainEvent(new OrganizationalUnitCreated { OrganizationalUnit = this });
    }


    /// <summary>
    /// Unique code for this organizational unit.
    /// Example: "HR-001", "DIST-DIV-001", "METER-SEC-001"
    /// </summary>
    public string Code { get; private set; } = default!;

    /// <summary>
    /// Type of organizational unit.
    /// </summary>
    public OrganizationalUnitType Type { get; private set; }

    /// <summary>
    /// Parent organizational unit ID.
    /// Null for top-level departments.
    /// </summary>
    public DefaultIdType? ParentId { get; private set; }
    public OrganizationalUnit? Parent { get; private set; }

    /// <summary>
    /// Child organizational units.
    /// </summary>
    public List<OrganizationalUnit> Children { get; private set; } = new();

    /// <summary>
    /// Hierarchy level (1=Department, 2=Division, 3=Section).
    /// </summary>
    public int Level { get; private set; }

    /// <summary>
    /// Materialized path for efficient hierarchy queries.
    /// Example: "/HR-001/DIST-001/METER-001/"
    /// </summary>
    public string? HierarchyPath { get; private set; }

    /// <summary>
    /// Manager/Head of this organizational unit.
    /// </summary>
    public DefaultIdType? ManagerId { get; private set; }

    /// <summary>
    /// Cost center code for accounting integration.
    /// </summary>
    public string? CostCenter { get; private set; }

    /// <summary>
    /// Physical location of this organizational unit.
    /// </summary>
    public string? Location { get; private set; }

    /// <summary>
    /// Whether this organizational unit is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Creates a new organizational unit.
    /// </summary>
    public static OrganizationalUnit Create(
        string code,
        string name,
        OrganizationalUnitType type,
        DefaultIdType? parentId = null,
        DefaultIdType? managerId = null,
        string? costCenter = null,
        string? location = null)
    {
        // Determine level based on type
        int level = type switch
        {
            OrganizationalUnitType.Department => 1,
            OrganizationalUnitType.Division => 2,
            OrganizationalUnitType.Section => 3,
            _ => throw new ArgumentException("Invalid organizational unit type")
        };

        var unit = new OrganizationalUnit(
            DefaultIdType.NewGuid(),
            code,
            name,
            type,
            parentId,
            level)
        {
            ManagerId = managerId,
            CostCenter = costCenter,
            Location = location
        };

        return unit;
    }

    /// <summary>
    /// Updates organizational unit information.
    /// </summary>
    public OrganizationalUnit Update(
        string? name,
        DefaultIdType? managerId,
        string? costCenter,
        string? location)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            Name = name;
            isUpdated = true;
        }

        if (ManagerId != managerId)
        {
            ManagerId = managerId;
            isUpdated = true;
        }

        if (!string.Equals(CostCenter, costCenter, StringComparison.OrdinalIgnoreCase))
        {
            CostCenter = costCenter;
            isUpdated = true;
        }

        if (!string.Equals(Location, location, StringComparison.OrdinalIgnoreCase))
        {
            Location = location;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new OrganizationalUnitUpdated { OrganizationalUnit = this });
        }

        return this;
    }

    /// <summary>
    /// Moves this organizational unit to a new parent.
    /// </summary>
    public OrganizationalUnit MoveTo(DefaultIdType? newParentId, int newLevel)
    {
        if (ParentId != newParentId)
        {
            ParentId = newParentId;
            Level = newLevel;
            QueueDomainEvent(new OrganizationalUnitMoved 
            { 
                OrganizationalUnitId = Id,
                NewParentId = newParentId,
                NewLevel = newLevel
            });
        }

        return this;
    }

    /// <summary>
    /// Updates the hierarchy path (called after parent changes).
    /// </summary>
    public void UpdateHierarchyPath(string path)
    {
        HierarchyPath = path;
    }

    /// <summary>
    /// Activates this organizational unit.
    /// </summary>
    public OrganizationalUnit Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
            QueueDomainEvent(new OrganizationalUnitActivated { OrganizationalUnitId = Id });
        }
        return this;
    }

    /// <summary>
    /// Deactivates this organizational unit.
    /// </summary>
    public OrganizationalUnit Deactivate()
    {
        if (IsActive)
        {
            IsActive = false;
            QueueDomainEvent(new OrganizationalUnitDeactivated { OrganizationalUnitId = Id });
        }
        return this;
    }
}

/// <summary>
/// Type of organizational unit.
/// </summary>
public enum OrganizationalUnitType
{
    Company = 0,
    Department = 1,
    Division = 2,
    Section = 3
}

