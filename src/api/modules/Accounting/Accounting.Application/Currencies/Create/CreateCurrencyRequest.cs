using MediatR;

namespace Accounting.Application.Currencies.Create;

public record CreateCurrencyRequest(
    string CurrencyCode,
    string Name,
    string Symbol,
    int DecimalPlaces = 2,
    string? Description = null,
    string? Notes = null) : IRequest<DefaultIdType>;
