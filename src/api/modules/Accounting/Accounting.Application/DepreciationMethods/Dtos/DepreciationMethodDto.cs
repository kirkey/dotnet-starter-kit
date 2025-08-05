namespace Accounting.Application.DepreciationMethods.Dtos;

public record DepreciationMethodDto(
    DefaultIdType Id,
    string MethodCode,
    string Name,
    string? Formula,
    bool IsActive,
    string? Description,
    string? Notes);
