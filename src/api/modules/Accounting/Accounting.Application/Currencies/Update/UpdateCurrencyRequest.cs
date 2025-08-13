using MediatR;

namespace Accounting.Application.Currencies.Update;

public class UpdateCurrencyRequest : IRequest<DefaultIdType>
{
    public DefaultIdType Id { get; set; }
    public string? CurrencyCode { get; set; }
    public string? Name { get; set; }
    public string? Symbol { get; set; }
    public int? DecimalPlaces { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsBaseCurrency { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }

    public UpdateCurrencyRequest(
        DefaultIdType id,
        string? currencyCode = null,
        string? name = null,
        string? symbol = null,
        int? decimalPlaces = null,
        bool? isActive = null,
        bool? isBaseCurrency = null,
        string? description = null,
        string? notes = null)
    {
        Id = id;
        CurrencyCode = currencyCode;
        Name = name;
        Symbol = symbol;
        DecimalPlaces = decimalPlaces;
        IsActive = isActive;
        IsBaseCurrency = isBaseCurrency;
        Description = description;
        Notes = notes;
    }
}
