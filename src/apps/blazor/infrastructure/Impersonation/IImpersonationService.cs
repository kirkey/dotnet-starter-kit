namespace FSH.Starter.Blazor.Infrastructure.Impersonation;
public interface IImpersonationService
{
    bool IsImpersonating { get; }
    string? OriginalUserId { get; }
    Task StartAsync(string targetUserId);
    Task EndAsync();
}
public class ImpersonationService : IImpersonationService
{
    public bool IsImpersonating { get; private set; }
    public string? OriginalUserId { get; private set; }
    public Task StartAsync(string targetUserId)
    {
        if (!IsImpersonating)
        {
            OriginalUserId = "current-user"; // placeholder - integrate with auth context
            IsImpersonating = true;
        }
        return Task.CompletedTask;
    }
    public Task EndAsync()
    {
        IsImpersonating = false;
        OriginalUserId = null;
        return Task.CompletedTask;
    }
}

