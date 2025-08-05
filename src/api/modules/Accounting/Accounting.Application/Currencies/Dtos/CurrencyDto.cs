namespace Accounting.Application.Currencies.Dtos;

public record CurrencyDto(
    DefaultIdType Id,
    string CurrencyCode,
    string Name,
    string Symbol,
    int DecimalPlaces,
    bool IsActive,
    bool IsBaseCurrency,
    string? Description,
    string? Notes);
