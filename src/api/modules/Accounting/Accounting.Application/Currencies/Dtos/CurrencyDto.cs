using FSH.Framework.Core.Extensions.Dto;

namespace Accounting.Application.Currencies.Dtos;

public class CurrencyDto : BaseDto
{
    public string CurrencyCode { get; set; } = null!;
    public string Symbol { get; set; } = null!;
    public int DecimalPlaces { get; set; }
    public bool IsActive { get; set; }
    public bool IsBaseCurrency { get; set; }
}
