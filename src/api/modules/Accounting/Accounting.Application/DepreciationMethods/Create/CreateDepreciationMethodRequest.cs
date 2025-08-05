using MediatR;

namespace Accounting.Application.DepreciationMethods.Create;

public record CreateDepreciationMethodRequest(
    string MethodCode,
    string MethodName,
    string CalculationFormula,
    string Description,
    string? Notes = null) : IRequest<DefaultIdType>;
