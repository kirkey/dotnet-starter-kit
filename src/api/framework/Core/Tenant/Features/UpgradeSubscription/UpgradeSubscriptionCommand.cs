using MediatR;

namespace FSH.Framework.Core.Tenant.Features.UpgradeSubscription;
public class UpgradeSubscriptionCommand : IRequest<UpgradeSubscriptionResponse>
{
    public string Tenant { get; set; } = null!;
    public DateTime ExtendedExpiryDate { get; set; }
}
