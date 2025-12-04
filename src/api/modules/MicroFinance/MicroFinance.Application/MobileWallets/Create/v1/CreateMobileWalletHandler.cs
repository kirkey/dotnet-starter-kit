using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Create.v1;

public sealed class CreateMobileWalletHandler(
    [FromKeyedServices("microfinance:mobilewallets")] IRepository<MobileWallet> repository,
    ILogger<CreateMobileWalletHandler> logger)
    : IRequestHandler<CreateMobileWalletCommand, CreateMobileWalletResponse>
{
    public async Task<CreateMobileWalletResponse> Handle(CreateMobileWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = MobileWallet.Create(
            request.MemberId,
            request.PhoneNumber,
            request.Provider,
            request.DailyLimit,
            request.MonthlyLimit);

        await repository.AddAsync(wallet, cancellationToken);
        logger.LogInformation("Mobile wallet created for phone {PhoneNumber} with ID {Id}", wallet.PhoneNumber, wallet.Id);

        return new CreateMobileWalletResponse(wallet.Id, wallet.PhoneNumber, wallet.Provider, wallet.Status);
    }
}
