using System.Collections.ObjectModel;
using FSH.Framework.Core.Audit;
using MediatR;

namespace FSH.Framework.Infrastructure.Identity.Audit;
public class AuditPublishedEvent(Collection<AuditTrail>? trails) : INotification
{
    public Collection<AuditTrail>? Trails { get; } = trails;
}
