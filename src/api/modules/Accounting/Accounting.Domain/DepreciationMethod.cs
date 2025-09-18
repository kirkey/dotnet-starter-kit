using Accounting.Domain.Events.DepreciationMethod;

namespace Accounting.Domain;

/// <summary>
/// Catalog of depreciation methods available for fixed assets (e.g., Straight-Line, Declining Balance).
/// </summary>
/// <remarks>
/// Stores a short code, display name, and the calculation formula expression or description. Defaults: <see cref="IsActive"/>
/// is set to true on creation. Methods can be activated/deactivated.
/// </remarks>
public class DepreciationMethod : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Short identifier for the method (e.g., SL, DB200).
    /// </summary>
    public string MethodCode { get; private set; }

    /// <summary>
    /// Formula or rule describing how depreciation is calculated.
    /// </summary>
    public string CalculationFormula { get; private set; }

    /// <summary>
    /// Whether this method is active and available to assign to assets.
    /// </summary>
    public bool IsActive { get; private set; }
    
    private DepreciationMethod()
    {
        // EF Core requires a parameterless constructor for entity instantiation
    }

    private DepreciationMethod(string methodCode, string methodName, string calculationFormula, string description, string? notes = null)
    {
        MethodCode = methodCode.Trim();
        Name = methodName.Trim();
        CalculationFormula = calculationFormula.Trim();
        IsActive = true;
        Description = description.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new DepreciationMethodCreated(Id, MethodCode, Name, CalculationFormula, Description, Notes));
    }

    /// <summary>
    /// Create a new depreciation method; defaults to active.
    /// </summary>
    public static DepreciationMethod Create(string methodCode, string methodName, string calculationFormula, string description, string? notes = null)
    {
        return new DepreciationMethod(methodCode, methodName, calculationFormula, description, notes);
    }

    /// <summary>
    /// Update metadata, formula and active flag.
    /// </summary>
    public DepreciationMethod Update(string? methodCode, string? methodName, string? calculationFormula, bool isActive = false, string? description = null, string? notes = null)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(methodCode) && MethodCode != methodCode)
        {
            MethodCode = methodCode.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(methodName) && Name != methodName)
        {
            Name = methodName.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(description) && Description != description)
        {
            Description = description.Trim();
            isUpdated = true;
        }
        
        if (!string.IsNullOrWhiteSpace(calculationFormula) && CalculationFormula != calculationFormula)
        {
            CalculationFormula = calculationFormula.Trim();
            isUpdated = true;
        }

        if (notes != Notes)
        {
            Notes = notes?.Trim();
            isUpdated = true;
        }

        if (IsActive != isActive)
        {
            IsActive = isActive;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new DepreciationMethodUpdated(this));
        }

        return this;
    }

    /// <summary>
    /// Activate this method.
    /// </summary>
    public DepreciationMethod Activate()
    {
        if (IsActive)
            throw new DepreciationMethodAlreadyActiveException(Id);

        IsActive = true;
        QueueDomainEvent(new DepreciationMethodActivated(Id, Name));
        return this;
    }

    /// <summary>
    /// Deactivate this method.
    /// </summary>
    public DepreciationMethod Deactivate()
    {
        if (!IsActive)
            throw new DepreciationMethodAlreadyInactiveException(Id);

        IsActive = false;
        QueueDomainEvent(new DepreciationMethodDeactivated(Id, Name));
        return this;
    }
}