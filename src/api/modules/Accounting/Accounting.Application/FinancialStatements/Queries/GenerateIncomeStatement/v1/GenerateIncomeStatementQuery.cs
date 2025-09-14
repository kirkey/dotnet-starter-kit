namespace Accounting.Application.FinancialStatements.Queries.GenerateIncomeStatement.v1;

public class GenerateIncomeStatementQuery : BaseRequest, IRequest<IncomeStatementDto>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DefaultIdType? AccountingPeriodId { get; set; }
    public string ReportFormat { get; set; } = "Standard"; // Standard, Detailed, Summary
    public bool IncludeComparativePeriod { get; set; }
    public DateTime? ComparativeStartDate { get; set; }
    public DateTime? ComparativeEndDate { get; set; }
}
