using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Search.v1;

/// <summary>
/// Specification for searching collection cases with comprehensive filtering capabilities.
/// Implements pagination and multiple search criteria following the CQRS pattern.
/// </summary>
public class SearchCollectionCasesSpec : EntitiesByPaginationFilterSpec<CollectionCase, CollectionCaseResponse>
{
    /// <summary>
    /// Initializes a new instance of the SearchCollectionCasesSpec class with search criteria.
    /// </summary>
    /// <param name="command">The search command containing filter criteria and pagination settings.</param>
    public SearchCollectionCasesSpec(SearchCollectionCasesCommand command)
        : base(command) =>
        Query
            .OrderByDescending(c => c.OpenedDate, !command.HasOrderBy())
            .ThenBy(c => c.CaseNumber)
            .Where(c => c.Status == command.Status!, !string.IsNullOrWhiteSpace(command.Status))
            .Where(c => c.LoanId == command.LoanId!.Value, command.LoanId.HasValue && command.LoanId.Value != DefaultIdType.Empty)
            .Where(c => c.MemberId == command.MemberId!.Value, command.MemberId.HasValue && command.MemberId.Value != DefaultIdType.Empty)
            .Where(c => c.AssignedCollectorId == command.AssignedToId!.Value, command.AssignedToId.HasValue && command.AssignedToId.Value != DefaultIdType.Empty)
            .Where(c => c.Priority == command.Priority!, !string.IsNullOrWhiteSpace(command.Priority))
            .Where(c => c.Classification == command.Classification!, !string.IsNullOrWhiteSpace(command.Classification))
            .Where(c => c.CaseNumber.Contains(command.Keyword!), !string.IsNullOrEmpty(command.Keyword));
}
