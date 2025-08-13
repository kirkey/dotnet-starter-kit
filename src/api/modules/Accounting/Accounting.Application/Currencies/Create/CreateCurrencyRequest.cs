using MediatR;

namespace Accounting.Application.Currencies.Create;

public class CreateCurrencyRequest : IRequest<DefaultIdType>
{
    public string CurrencyCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Symbol { get; set; } = null!;
    public int DecimalPlaces { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }

    public CreateCurrencyRequest(
        string currencyCode,
        string name,
        string symbol,
        int decimalPlaces = 2,
        string? description = null,
        string? notes = null)
    {
        CurrencyCode = currencyCode;
        Name = name;
        Symbol = symbol;
        DecimalPlaces = decimalPlaces;
        Description = description;
        Notes = notes;
    }
}
