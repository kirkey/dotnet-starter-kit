using FSH.Starter.WebApi.HumanResources.Domain.Events;

namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents a job designation within a specific organizational unit (area/department).
/// </summary>
/// <remarks>
/// Key Design Points:
/// - Designations are organizational unit specific (different per area)
/// - Same designation title can exist in multiple areas with different designation records
/// - Each designation can have different salary ranges per area
/// - Unique constraint: Code per Area (OrganizationalUnit) per Tenant
/// 
/// Example:
/// - Area 1: Designation "Supervisor" with MinSalary 40K-55K
/// - Area 2: Designation "Supervisor" with MinSalary 42K-58K
/// Both have same title but different Designation entities
/// </remarks>
public class Designation : AuditableEntity, IAggregateRoot
{
    private Designation() { }

    private Designation(
        DefaultIdType id,
        string code,
        string title,
        DefaultIdType organizationalUnitId,
        string? description)
    {
        Id = id;
        Code = code;
        Title = title;
        OrganizationalUnitId = organizationalUnitId;
        Description = description;
        IsActive = true;

        QueueDomainEvent(new DesignationCreated { Designation = this });
    }

    /// <summary>
    /// Unique code for this designation within the organizational unit.
    /// Example: "SUP-001", "TECH-001"
    /// </summary>
    public string Code { get; private set; } = default!;

    /// <summary>
    /// Designation title/job title.
    /// Example: "Area Supervisor", "Line Technician", "Field Helper"
    /// Same title can exist in different areas.
    /// </summary>
    public string Title { get; private set; } = default!;

    /// <summary>
    /// Detailed job description for this designation in this organizational unit.
    /// Can vary per area.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// The organizational unit (Area/Department/Division/Section) this designation belongs to.
    /// Foreign key to OrganizationalUnit.
    /// 
    /// This is the KEY difference:
    /// - Same title "Supervisor" can exist in Area1, Area2, Area3
    /// - Each is a separate Designation record with different salary ranges
    /// - Linked to different OrganizationalUnit
    /// </summary>
    public DefaultIdType OrganizationalUnitId { get; private set; }
    public OrganizationalUnit OrganizationalUnit { get; private set; } = default!;

    /// <summary>
    /// Minimum salary for this designation in this area.
    /// Can vary by area due to cost of living, location, or market differences.
    /// </summary>
    public decimal? MinSalary { get; private set; }

    /// <summary>
    /// Maximum salary for this designation in this area.
    /// </summary>
    public decimal? MaxSalary { get; private set; }

    /// <summary>
    /// Whether this designation is currently active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Creates a new designation in a specific organizational unit.
    /// </summary>
    public static Designation Create(
        string code,
        string title,
        DefaultIdType organizationalUnitId,
        string? description = null,
        decimal? minSalary = null,
        decimal? maxSalary = null)
    {
        var designation = new Designation(
            DefaultIdType.NewGuid(),
            code,
            title,
            organizationalUnitId,
            description)
        {
            MinSalary = minSalary,
            MaxSalary = maxSalary
        };

        return designation;
    }

    /// <summary>
    /// Updates designation information.
    /// </summary>
    public Designation Update(
        string? title,
        string? description,
        decimal? minSalary,
        decimal? maxSalary)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(title) && !string.Equals(Title, title, StringComparison.OrdinalIgnoreCase))
        {
            Title = title;
            isUpdated = true;
        }

        if (!string.Equals(Description, description, StringComparison.OrdinalIgnoreCase))
        {
            Description = description;
            isUpdated = true;
        }

        if (MinSalary != minSalary)
        {
            MinSalary = minSalary;
            isUpdated = true;
        }

        if (MaxSalary != maxSalary)
        {
            MaxSalary = maxSalary;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new DesignationUpdated { Designation = this });
        }

        return this;
    }

    /// <summary>
    /// Activates this designation.
    /// </summary>
    public Designation Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
            QueueDomainEvent(new DesignationActivated { DesignationId = Id });
        }
        return this;
    }

    /// <summary>
    /// Deactivates this designation.
    /// </summary>
    public Designation Deactivate()
    {
        if (IsActive)
        {
            IsActive = false;
            QueueDomainEvent(new DesignationDeactivated { DesignationId = Id });
        }
        return this;
    }
}

