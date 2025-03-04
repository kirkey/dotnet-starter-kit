using FSH.Framework.Core.Tenant.Abstractions;
using MediatR;

namespace FSH.Framework.Core.Tenant.Features.UpgradeSubscription;

public class UpgradeSubscriptionHandler(ITenantService tenantService) : IRequestHandler<UpgradeSubscriptionCommand, UpgradeSubscriptionResponse>
{
    private readonly ITenantService _tenantService = tenantService;

    public async Task<UpgradeSubscriptionResponse> Handle(UpgradeSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var validUpto = await _tenantService.UpgradeSubscription(request.Tenant, request.ExtendedExpiryDate).ConfigureAwait(false);
        return new UpgradeSubscriptionResponse(validUpto, request.Tenant);
    }
}
