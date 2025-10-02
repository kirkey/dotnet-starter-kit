namespace Accounting.Application.TaxCodes.Create.v1;

public class CreateTaxCodeCommand : BaseRequest, IRequest<DefaultIdType>
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string TaxType { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public bool IsCompound { get; set; }
    public string? Jurisdiction { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public DefaultIdType TaxCollectedAccountId { get; set; }
    public DefaultIdType? TaxPaidAccountId { get; set; }
    public string? TaxAuthority { get; set; }
    public string? TaxRegistrationNumber { get; set; }
    public string? ReportingCategory { get; set; }
}
