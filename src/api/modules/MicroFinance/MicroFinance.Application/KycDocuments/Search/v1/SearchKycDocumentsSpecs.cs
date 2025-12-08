using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.KycDocuments.Search.v1;

public class SearchKycDocumentsSpecs : EntitiesByPaginationFilterSpec<KycDocument, KycDocumentSummaryResponse>
{
    public SearchKycDocumentsSpecs(SearchKycDocumentsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.CreatedOn, !command.HasOrderBy())
            .Where(x => x.MemberId == command.MemberId!.Value, command.MemberId.HasValue)
            .Where(x => x.DocumentType == command.DocumentType, !string.IsNullOrWhiteSpace(command.DocumentType))
            .Where(x => x.DocumentNumber!.Contains(command.DocumentNumber!, StringComparison.OrdinalIgnoreCase), !string.IsNullOrWhiteSpace(command.DocumentNumber))
            .Where(x => x.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(x => x.ExpiryDate >= command.ExpiryDateFrom!.Value, command.ExpiryDateFrom.HasValue)
            .Where(x => x.ExpiryDate <= command.ExpiryDateTo!.Value, command.ExpiryDateTo.HasValue);
}
