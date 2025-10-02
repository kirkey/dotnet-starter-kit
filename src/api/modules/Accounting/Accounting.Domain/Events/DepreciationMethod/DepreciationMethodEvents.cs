namespace Accounting.Domain.Events.DepreciationMethod;

public record DepreciationMethodCreated(DefaultIdType Id, string MethodCode, string MethodName, string CalculationFormula, string Description, string? Notes) : DomainEvent;

public record DepreciationMethodUpdated(Entities.DepreciationMethod Method) : DomainEvent;

public record DepreciationMethodDeleted(DefaultIdType Id) : DomainEvent;

public record DepreciationMethodActivated(DefaultIdType Id, string MethodName) : DomainEvent;

public record DepreciationMethodDeactivated(DefaultIdType Id, string MethodName) : DomainEvent;
