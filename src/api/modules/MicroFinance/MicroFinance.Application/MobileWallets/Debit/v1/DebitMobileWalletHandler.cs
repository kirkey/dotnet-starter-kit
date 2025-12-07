using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Debit.v1;

public sealed class DebitMobileWalletHandler(
    [FromKeyedServices("microfinance:mobilewallets")] IRepository<MobileWallet> repository,
    ILogger<DebitMobileWalletHandler> logger)
    : IRequestHandler<DebitMobileWalletCommand, DebitMobileWalletResponse>
{
    public async Task<DebitMobileWalletResponse> Handle(DebitMobileWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = await repository.FirstOrDefaultAsync(new MobileWalletByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Mobile wallet {request.Id} not found");

        wallet.Debit(request.Amount, request.TransactionReference);
        await repository.UpdateAsync(wallet, cancellationToken);

        logger.LogInformation("Debited {Amount} from mobile wallet {Id}", request.Amount, wallet.Id);

        return new DebitMobileWalletResponse(wallet.Id, wallet.Balance);
    }
}
