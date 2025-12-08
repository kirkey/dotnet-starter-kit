using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSurveys.Search.v1;

public class SearchCustomerSurveysSpecs : EntitiesByPaginationFilterSpec<CustomerSurvey, CustomerSurveySummaryResponse>
{
    public SearchCustomerSurveysSpecs(SearchCustomerSurveysCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.StartDate, !command.HasOrderBy())
            .Where(x => x.Title.Contains(command.Title!, StringComparison.OrdinalIgnoreCase), !string.IsNullOrWhiteSpace(command.Title))
            .Where(x => x.SurveyType == command.SurveyType, !string.IsNullOrWhiteSpace(command.SurveyType))
            .Where(x => x.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(x => x.BranchId == command.BranchId, command.BranchId.HasValue)
            .Where(x => x.StartDate >= command.StartDateFrom!.Value, command.StartDateFrom.HasValue)
            .Where(x => x.StartDate <= command.StartDateTo!.Value, command.StartDateTo.HasValue)
            .Where(x => x.EndDate >= command.EndDateFrom!.Value, command.EndDateFrom.HasValue)
            .Where(x => x.EndDate <= command.EndDateTo!.Value, command.EndDateTo.HasValue)
            .Where(x => x.IsAnonymous == command.IsAnonymous!.Value, command.IsAnonymous.HasValue);
}
