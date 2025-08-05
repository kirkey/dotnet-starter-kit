using MediatR;

namespace Accounting.Application.Currencies.Update;

public record UpdateCurrencyRequest(
    DefaultIdType Id,
    string? CurrencyCode = null,
    string? Name = null,
    string? Symbol = null,
    int? DecimalPlaces = null,
    bool? IsActive = null,
    bool? IsBaseCurrency = null,
    string? Description = null,
    string? Notes = null) : IRequest<DefaultIdType>;
