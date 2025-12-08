// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/QrPayments/Search/v1/SearchQrPaymentsHandler.cs
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.QrPayments.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.QrPayments.Search.v1;

/// <summary>
/// Handler for searching QR payments.
/// </summary>
public sealed class SearchQrPaymentsHandler(
    [FromKeyedServices("microfinance:qrpayments")] IReadRepository<QrPayment> repository)
    : IRequestHandler<SearchQrPaymentsCommand, PagedList<QrPaymentResponse>>
{
    public async Task<PagedList<QrPaymentResponse>> Handle(SearchQrPaymentsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchQrPaymentsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<QrPaymentResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

