namespace Accounting.Application.Billing.Commands;

public sealed class CreateInvoiceFromConsumptionCommand : IRequest<DefaultIdType>
{
    public DefaultIdType ConsumptionId { get; set; }
    public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
    public int DueDays { get; set; } = 15;
}

