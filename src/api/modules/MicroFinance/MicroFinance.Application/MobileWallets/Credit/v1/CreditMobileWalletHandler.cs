using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Credit.v1;

public sealed class CreditMobileWalletHandler(
    [FromKeyedServices("microfinance:mobilewallets")] IRepository<MobileWallet> repository,
    ILogger<CreditMobileWalletHandler> logger)
    : IRequestHandler<CreditMobileWalletCommand, CreditMobileWalletResponse>
{
    public async Task<CreditMobileWalletResponse> Handle(CreditMobileWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = await repository.FirstOrDefaultAsync(new MobileWalletByIdSpec(request.Id), cancellationToken)
            ?? throw new Exception($"Mobile wallet {request.Id} not found");

        wallet.Credit(request.Amount, request.TransactionReference);
        await repository.UpdateAsync(wallet, cancellationToken);

        logger.LogInformation("Credited {Amount} to mobile wallet {Id}", request.Amount, wallet.Id);

        return new CreditMobileWalletResponse(wallet.Id, wallet.Balance);
    }
}
