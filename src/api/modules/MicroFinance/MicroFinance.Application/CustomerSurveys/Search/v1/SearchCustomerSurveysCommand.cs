using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSurveys.Search.v1;

public class SearchCustomerSurveysCommand : PaginationFilter, IRequest<PagedList<CustomerSurveySummaryResponse>>
{
    public string? Title { get; set; }
    public string? SurveyType { get; set; }
    public string? Status { get; set; }
    public DefaultIdType? BranchId { get; set; }
    public DateOnly? StartDateFrom { get; set; }
    public DateOnly? StartDateTo { get; set; }
    public DateOnly? EndDateFrom { get; set; }
    public DateOnly? EndDateTo { get; set; }
    public bool? IsAnonymous { get; set; }
}

public sealed record CustomerSurveySummaryResponse(
    DefaultIdType Id,
    string Title,
    string SurveyType,
    string Status,
    DateOnly StartDate,
    DateOnly? EndDate,
    int TotalResponses,
    decimal? AverageScore,
    int? NpsScore,
    bool IsAnonymous);
