using Accounting.Domain.Events.DepreciationMethod;
using Accounting.Domain.Exceptions;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace Accounting.Domain;

public class DepreciationMethod : AuditableEntity, IAggregateRoot
{
    public string MethodCode { get; private set; }
    public string CalculationFormula { get; private set; }
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

    public static DepreciationMethod Create(string methodCode, string methodName, string calculationFormula, string description, string? notes = null)
    {
        return new DepreciationMethod(methodCode, methodName, calculationFormula, description, notes);
    }

    public DepreciationMethod Update(string? methodCode, string? methodName, string? calculationFormula, bool? isActive, string? description, string? notes)
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

        if (isActive.HasValue && IsActive != isActive.Value)
        {
            IsActive = isActive.Value;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new DepreciationMethodUpdated(this));
        }

        return this;
    }

    public DepreciationMethod Activate()
    {
        if (IsActive)
            throw new DepreciationMethodAlreadyActiveException(Id);

        IsActive = true;
        QueueDomainEvent(new DepreciationMethodActivated(Id, Name));
        return this;
    }

    public DepreciationMethod Deactivate()
    {
        if (!IsActive)
            throw new DepreciationMethodAlreadyInactiveException(Id);

        IsActive = false;
        QueueDomainEvent(new DepreciationMethodDeactivated(Id, Name));
        return this;
    }
}
