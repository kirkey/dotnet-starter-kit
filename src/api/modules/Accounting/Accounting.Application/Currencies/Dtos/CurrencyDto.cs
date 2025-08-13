namespace Accounting.Application.Currencies.Dtos;

public class CurrencyDto
{
    public DefaultIdType Id { get; set; }
    public string CurrencyCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Symbol { get; set; } = null!;
    public int DecimalPlaces { get; set; }
    public bool IsActive { get; set; }
    public bool IsBaseCurrency { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }

    public CurrencyDto(
        DefaultIdType id,
        string currencyCode,
        string name,
        string symbol,
        int decimalPlaces,
        bool isActive,
        bool isBaseCurrency,
        string? description,
        string? notes)
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
