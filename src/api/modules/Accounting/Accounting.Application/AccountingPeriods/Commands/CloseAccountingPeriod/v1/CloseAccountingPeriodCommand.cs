namespace Accounting.Application.AccountingPeriods.Commands.CloseAccountingPeriod.v1;

public class CloseAccountingPeriodCommand : BaseRequest, IRequest<DefaultIdType>
{
    public DefaultIdType AccountingPeriodId { get; set; }
    public DateTime ClosingDate { get; set; }
    public string? ClosingNotes { get; set; }
    public bool PerformYearEndAdjustments { get; set; }
    public bool GenerateClosingEntries { get; set; } = true;
}
