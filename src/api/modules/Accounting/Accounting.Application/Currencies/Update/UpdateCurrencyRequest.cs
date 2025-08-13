using MediatR;

namespace Accounting.Application.Currencies.Update;

public class UpdateCurrencyRequest(
    DefaultIdType id,
    string? currencyCode = null,
    string? name = null,
    string? symbol = null,
    int? decimalPlaces = null,
    bool? isActive = null,
    bool? isBaseCurrency = null,
    string? description = null,
    string? notes = null)
    : IRequest<DefaultIdType>
{
    public DefaultIdType Id { get; set; } = id;
    public string? CurrencyCode { get; set; } = currencyCode;
    public string? Name { get; set; } = name;
    public string? Symbol { get; set; } = symbol;
    public int? DecimalPlaces { get; set; } = decimalPlaces;
    public bool? IsActive { get; set; } = isActive;
    public bool? IsBaseCurrency { get; set; } = isBaseCurrency;
    public string? Description { get; set; } = description;
    public string? Notes { get; set; } = notes;
}
