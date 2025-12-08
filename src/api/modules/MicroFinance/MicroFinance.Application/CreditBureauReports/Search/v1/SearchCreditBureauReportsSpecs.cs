using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditBureauReports.Search.v1;

public class SearchCreditBureauReportsSpecs : EntitiesByPaginationFilterSpec<CreditBureauReport, CreditBureauReportSummaryResponse>
{
    public SearchCreditBureauReportsSpecs(SearchCreditBureauReportsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.ReportDate, !command.HasOrderBy())
            .Where(x => x.MemberId == command.MemberId!.Value, command.MemberId.HasValue)
            .Where(x => x.InquiryId == command.InquiryId!.Value, command.InquiryId.HasValue)
            .Where(x => x.ReportNumber.Contains(command.ReportNumber!, StringComparison.OrdinalIgnoreCase), !string.IsNullOrWhiteSpace(command.ReportNumber))
            .Where(x => x.BureauName.Contains(command.BureauName!, StringComparison.OrdinalIgnoreCase), !string.IsNullOrWhiteSpace(command.BureauName))
            .Where(x => x.RiskGrade == command.RiskGrade, !string.IsNullOrWhiteSpace(command.RiskGrade))
            .Where(x => x.ReportDate >= command.ReportDateFrom!.Value, command.ReportDateFrom.HasValue)
            .Where(x => x.ReportDate <= command.ReportDateTo!.Value, command.ReportDateTo.HasValue)
            .Where(x => x.CreditScore >= command.CreditScoreMin!.Value, command.CreditScoreMin.HasValue)
            .Where(x => x.CreditScore <= command.CreditScoreMax!.Value, command.CreditScoreMax.HasValue);
}
