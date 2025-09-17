namespace Accounting.Application.DepreciationMethods.Update;

public record UpdateDepreciationMethodRequest(
    DefaultIdType Id,
    string? MethodCode = null,
    string? MethodName = null,
    string? CalculationFormula = null,
    bool IsActive = false,
    string? Description = null,
    string? Notes = null) : IRequest<DefaultIdType>;
