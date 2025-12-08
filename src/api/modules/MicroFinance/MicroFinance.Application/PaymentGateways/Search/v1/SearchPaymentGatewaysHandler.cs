// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/PaymentGateways/Search/v1/SearchPaymentGatewaysHandler.cs
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Search.v1;

/// <summary>
/// Handler for searching payment gateways.
/// </summary>
public sealed class SearchPaymentGatewaysHandler(
    [FromKeyedServices("microfinance:paymentgateways")] IReadRepository<PaymentGateway> repository)
    : IRequestHandler<SearchPaymentGatewaysCommand, PagedList<PaymentGatewayResponse>>
{
    public async Task<PagedList<PaymentGatewayResponse>> Handle(SearchPaymentGatewaysCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchPaymentGatewaysSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<PaymentGatewayResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

