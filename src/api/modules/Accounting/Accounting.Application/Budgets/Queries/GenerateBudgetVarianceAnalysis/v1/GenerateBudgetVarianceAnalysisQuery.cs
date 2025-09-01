using FSH.Framework.Core.Extensions.Dto;
using MediatR;

namespace Accounting.Application.Budgets.Queries.GenerateBudgetVarianceAnalysis.v1;

public class GenerateBudgetVarianceAnalysisQuery : BaseRequest, IRequest<BudgetVarianceAnalysisDto>
{
    public DefaultIdType BudgetId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string AnalysisType { get; set; } = "Detailed"; // Summary, Detailed, ByCategory
    public bool IncludePercentages { get; set; } = true;
    public decimal VarianceThreshold { get; set; } = 0; // Only show variances above this amount
}
