using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Get.v1;

public sealed class GetMobileWalletHandler(
    [FromKeyedServices("microfinance:mobilewallets")] IReadRepository<MobileWallet> repository,
    ILogger<GetMobileWalletHandler> logger)
    : IRequestHandler<GetMobileWalletRequest, MobileWalletResponse>
{
    public async Task<MobileWalletResponse> Handle(GetMobileWalletRequest request, CancellationToken cancellationToken)
    {
        var wallet = await repository.FirstOrDefaultAsync(new MobileWalletByIdSpec(request.Id), cancellationToken)
            ?? throw new Exception($"Mobile wallet {request.Id} not found");

        logger.LogInformation("Retrieved mobile wallet {Id}", wallet.Id);

        return new MobileWalletResponse(
            wallet.Id,
            wallet.MemberId,
            wallet.PhoneNumber,
            wallet.Provider,
            wallet.ExternalWalletId,
            wallet.Status,
            wallet.Tier,
            wallet.Balance,
            wallet.DailyLimit,
            wallet.MonthlyLimit,
            wallet.DailyUsed,
            wallet.MonthlyUsed,
            wallet.LastActivityDate,
            wallet.IsLinkedToBankAccount,
            wallet.LinkedSavingsAccountId);
    }
}
