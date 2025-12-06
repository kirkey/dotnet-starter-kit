using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Search.v1;

/// <summary>
/// Specification for searching AML alerts with comprehensive filtering capabilities.
/// Implements pagination and multiple search criteria following the CQRS pattern.
/// </summary>
public class SearchAmlAlertsSpec : EntitiesByPaginationFilterSpec<AmlAlert, AmlAlertResponse>
{
    /// <summary>
    /// Initializes a new instance of the SearchAmlAlertsSpec class with search criteria.
    /// </summary>
    /// <param name="command">The search command containing filter criteria and pagination settings.</param>
    public SearchAmlAlertsSpec(SearchAmlAlertsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(a => a.AlertedAt, !command.HasOrderBy())
            .ThenBy(a => a.AlertCode)
            .Where(a => a.Status == command.Status!, !string.IsNullOrWhiteSpace(command.Status))
            .Where(a => a.Severity == command.Severity!, !string.IsNullOrWhiteSpace(command.Severity))
            .Where(a => a.AlertType == command.AlertType!, !string.IsNullOrWhiteSpace(command.AlertType))
            .Where(a => a.MemberId == command.MemberId!.Value, command.MemberId.HasValue && command.MemberId.Value != Guid.Empty)
            .Where(a => a.AlertCode.Contains(command.Keyword!) ||
                       a.Description.Contains(command.Keyword!) ||
                       a.TriggerRule.Contains(command.Keyword!),
                   !string.IsNullOrEmpty(command.Keyword));
}
