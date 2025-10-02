using Accounting.Application.BankReconciliations.Responses;

namespace Accounting.Application.BankReconciliations.Search.v1;

public sealed class SearchBankReconciliationsHandler(
    IReadRepository<BankReconciliation> repository)
    : IRequestHandler<SearchBankReconciliationsCommand, PagedList<BankReconciliationResponse>>
{
    public async Task<PagedList<BankReconciliationResponse>> Handle(SearchBankReconciliationsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchBankReconciliationsSpec(request);
        var items = await repository.ListAsync(spec, cancellationToken);
        var totalCount = await repository.CountAsync(spec, cancellationToken);

        return new PagedList<BankReconciliationResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
