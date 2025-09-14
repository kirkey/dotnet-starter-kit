using Accounting.Application.Payees.Get.v1;

namespace Accounting.Application.Payees.Search.v1;
public sealed class PayeeSearchHandler(
    [FromKeyedServices("accounting:payees")] IReadRepository<Payee> repository)
    : IRequestHandler<PayeeSearchCommand, PagedList<PayeeResponse>>
{
    public async Task<PagedList<PayeeResponse>> Handle(PayeeSearchCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new PayeeSearchSpecs(request);

        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<PayeeResponse>(list, request.PageNumber, request.PageSize, totalCount);
    }
}

