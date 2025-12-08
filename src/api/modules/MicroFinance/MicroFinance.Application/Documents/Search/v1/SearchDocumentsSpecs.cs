using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.Documents.Search.v1;

public class SearchDocumentsSpecs : EntitiesByPaginationFilterSpec<Document, DocumentSummaryResponse>
{
    public SearchDocumentsSpecs(SearchDocumentsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.CreatedOn, !command.HasOrderBy())
            .Where(x => x.DocumentType == command.DocumentType, !string.IsNullOrWhiteSpace(command.DocumentType))
            .Where(x => x.Category == command.Category, !string.IsNullOrWhiteSpace(command.Category))
            .Where(x => x.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(x => x.EntityType == command.EntityType, !string.IsNullOrWhiteSpace(command.EntityType))
            .Where(x => x.EntityId == command.EntityId!.Value, command.EntityId.HasValue)
            .Where(x => x.IssueDate >= command.IssueDateFrom!.Value, command.IssueDateFrom.HasValue)
            .Where(x => x.IssueDate <= command.IssueDateTo!.Value, command.IssueDateTo.HasValue)
            .Where(x => x.ExpiryDate >= command.ExpiryDateFrom!.Value, command.ExpiryDateFrom.HasValue)
            .Where(x => x.ExpiryDate <= command.ExpiryDateTo!.Value, command.ExpiryDateTo.HasValue)
            .Where(x => x.IsVerified == command.IsVerified!.Value, command.IsVerified.HasValue)
            .Where(x => x.IsRequired == command.IsRequired!.Value, command.IsRequired.HasValue);
}
