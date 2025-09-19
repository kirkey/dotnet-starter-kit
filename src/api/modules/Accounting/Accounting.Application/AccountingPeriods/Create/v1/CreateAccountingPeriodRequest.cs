namespace Accounting.Application.AccountingPeriods.Create.v1;

public class CreateAccountingPeriodCommand(
    int year,
    int month,
    DateTime startDate,
    DateTime endDate,
    bool isClosed = false,
    string? notes = null) : IRequest<DefaultIdType>
{
    public int Year { get; set; } = year;
    public int Month { get; set; } = month;
    public DateTime StartDate { get; set; } = startDate;
    public DateTime EndDate { get; set; } = endDate;
    public bool IsClosed { get; set; } = isClosed;
    public string? Notes { get; set; } = notes;
}
