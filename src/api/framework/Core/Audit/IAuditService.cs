namespace FSH.Framework.Core.Audit;
public interface IAuditService
{
    Task<List<AuditTrail>> GetUserTrailsAsync(DefaultIdType userId);
}
