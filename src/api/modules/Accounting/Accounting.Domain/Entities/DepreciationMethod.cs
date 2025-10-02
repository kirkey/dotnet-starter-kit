using Accounting.Domain.Events.DepreciationMethod;

namespace Accounting.Domain.Entities;

/// <summary>
/// Defines depreciation calculation methods available for fixed asset depreciation schedules.
/// </summary>
/// <remarks>
/// Use cases:
/// - Configure standard depreciation methods for regulatory compliance (GAAP, IFRS, tax reporting).
/// - Support multiple depreciation methods like straight-line, declining balance, sum-of-years digits.
/// - Enable custom depreciation formulas for specialized asset types or regulatory requirements.
/// - Maintain consistent depreciation calculations across all fixed assets.
/// - Support method activation/deactivation for lifecycle management.
/// - Enable audit trail for depreciation method changes and their impact on assets.
/// 
/// Default values:
/// - MethodCode: required unique identifier (example: "SL" for straight-line, "DB200" for double declining)
/// - CalculationFormula: required formula description (example: "(Cost - Salvage) / Useful Life")
/// - IsActive: true (new methods are active by default)
/// - Name: inherited display name (example: "Straight-Line Depreciation")
/// - Description: inherited detailed description (example: "Equal annual depreciation over asset useful life")
/// 
/// Common depreciation methods:
/// - Straight-Line (SL): (Cost - Salvage Value) / Useful Life
/// - Double Declining Balance (DB200): Book Value × (2 / Useful Life)
/// - Sum-of-Years Digits (SYD): Remaining Life / Sum of Years × (Cost - Salvage)
/// - Units of Production (UOP): (Cost - Salvage) × (Units Produced / Total Expected Units)
/// 
/// Business rules:
/// - MethodCode must be unique within the system
/// - Cannot deactivate method if assigned to active assets
/// - CalculationFormula should be clear and auditable
/// - Method changes require approval for regulatory compliance
/// </remarks>
/// <seealso cref="Accounting.Domain.Events.DepreciationMethod.DepreciationMethodCreated"/>
/// <seealso cref="Accounting.Domain.Events.DepreciationMethod.DepreciationMethodUpdated"/>
/// <seealso cref="Accounting.Domain.Events.DepreciationMethod.DepreciationMethodActivated"/>
/// <seealso cref="Accounting.Domain.Events.DepreciationMethod.DepreciationMethodDeactivated"/>
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
