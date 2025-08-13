using MediatR;

namespace Accounting.Application.Currencies.Create;

public class CreateCurrencyRequest(
    string currencyCode,
    string name,
    string symbol,
    int decimalPlaces = 2,
    string? description = null,
    string? notes = null) : IRequest<DefaultIdType>
{
    public string CurrencyCode { get; set; } = currencyCode;
    public string Name { get; set; } = name;
    public string Symbol { get; set; } = symbol;
    public int DecimalPlaces { get; set; } = decimalPlaces;
    public string? Description { get; set; } = description;
    public string? Notes { get; set; } = notes;
}
