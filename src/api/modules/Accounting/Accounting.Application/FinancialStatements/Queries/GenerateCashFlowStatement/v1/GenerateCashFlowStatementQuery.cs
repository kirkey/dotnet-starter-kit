namespace Accounting.Application.FinancialStatements.Queries.GenerateCashFlowStatement.v1;

public class GenerateCashFlowStatementQuery : BaseRequest, IRequest<CashFlowStatementDto>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DefaultIdType? AccountingPeriodId { get; set; }
    public string Method { get; set; } = "Direct"; // Direct, Indirect
    public bool IncludeComparativePeriod { get; set; }
    public DateTime? ComparativeStartDate { get; set; }
    public DateTime? ComparativeEndDate { get; set; }
}
