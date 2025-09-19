namespace Accounting.Application.Accruals.Create;

public class CreateAccrualCommand(
    string referenceNumber,
    DateTime accrualDate,
    DefaultIdType? accountId,
    decimal amount,
    string? description = null,
    string? notes = null) : IRequest<DefaultIdType>
{
    public string ReferenceNumber { get; set; } = referenceNumber;
    public DateTime AccrualDate { get; set; } = accrualDate;
    public DefaultIdType? AccountId { get; set; } = accountId;
    public decimal Amount { get; set; } = amount;
    public string? Description { get; set; } = description;
    public string? Notes { get; set; } = notes;
}
