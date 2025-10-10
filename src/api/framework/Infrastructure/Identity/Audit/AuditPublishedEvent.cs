using System.Collections.ObjectModel;

namespace FSH.Framework.Infrastructure.Identity.Audit;
public class AuditPublishedEvent(Collection<AuditTrail>? trails) : INotification
{
    public Collection<AuditTrail>? Trails { get; } = trails;
}
