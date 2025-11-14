namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents a pay component (earnings type or deduction type).
/// Used for configuration of what goes into payroll calculations.
/// </summary>
public class PayComponent : AuditableEntity, IAggregateRoot
{
    private PayComponent() { }

    private PayComponent(
        DefaultIdType id,
        string componentName,
        string componentType,
        string glAccountCode = "")
    {
        Id = id;
        ComponentName = componentName;
        ComponentType = componentType;
        GLAccountCode = glAccountCode;
        IsActive = true;
        IsCalculated = false;
    }

    /// <summary>
    /// Gets the name of the pay component.
    /// </summary>
    public string ComponentName { get; private set; } = default!;

    /// <summary>
    /// Gets the type of component (Earnings, Tax, Deduction).
    /// </summary>
    public string ComponentType { get; private set; } = default!;

    /// <summary>
    /// Gets the GL account code for posting.
    /// </summary>
    public string GLAccountCode { get; private set; } = default!;

    /// <summary>
    /// Gets a value indicating whether the component is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the component is auto-calculated.
    /// </summary>
    public bool IsCalculated { get; private set; }

    /// <summary>
    /// Gets the description of the component.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Creates a new pay component.
    /// </summary>
    public static PayComponent Create(
        string componentName,
        string componentType,
        string glAccountCode = "")
    {
        if (string.IsNullOrWhiteSpace(componentName))
            throw new ArgumentException("Component name is required.", nameof(componentName));

        var component = new PayComponent(
            DefaultIdType.NewGuid(),
            componentName,
            componentType,
            glAccountCode);

        return component;
    }

    /// <summary>
    /// Updates the pay component information.
    /// </summary>
    public PayComponent Update(string? componentName = null, string? glAccountCode = null, string? description = null)
    {
        if (!string.IsNullOrWhiteSpace(componentName))
            ComponentName = componentName;

        if (!string.IsNullOrWhiteSpace(glAccountCode))
            GLAccountCode = glAccountCode;

        if (description != null)
            Description = description;

        return this;
    }

    /// <summary>
    /// Deactivates the pay component.
    /// </summary>
    public PayComponent Deactivate()
    {
        IsActive = false;
        return this;
    }

    /// <summary>
    /// Activates the pay component.
    /// </summary>
    public PayComponent Activate()
    {
        IsActive = true;
        return this;
    }
}
