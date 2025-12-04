using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a USSD session for mobile banking.
/// </summary>
public sealed class UssdSession : AuditableEntity, IAggregateRoot
{
    // Constants
    public const int SessionIdMaxLength = 64;
    public const int PhoneNumberMaxLength = 32;
    public const int ServiceCodeMaxLength = 16;
    public const int StatusMaxLength = 32;
    public const int CurrentMenuMaxLength = 64;
    public const int LanguageMaxLength = 8;
    public const int OperationMaxLength = 64;
    public const int DataMaxLength = 4096;
    
    // Session Status
    public const string StatusActive = "Active";
    public const string StatusCompleted = "Completed";
    public const string StatusTimedOut = "TimedOut";
    public const string StatusCancelled = "Cancelled";
    public const string StatusError = "Error";

    public string SessionId { get; private set; } = default!;
    public string PhoneNumber { get; private set; } = default!;
    public string ServiceCode { get; private set; } = default!;
    public Guid? MemberId { get; private set; }
    public Guid? WalletId { get; private set; }
    public string Status { get; private set; } = StatusActive;
    public string CurrentMenu { get; private set; } = "MainMenu";
    public string? Language { get; private set; }
    public string? CurrentOperation { get; private set; }
    public string? SessionData { get; private set; }
    public int MenuLevel { get; private set; }
    public int StepCount { get; private set; }
    public DateTimeOffset StartedAt { get; private set; }
    public DateTimeOffset? EndedAt { get; private set; }
    public DateTimeOffset LastActivityAt { get; private set; }
    public int SessionTimeoutSeconds { get; private set; } = 180;
    public string? LastInput { get; private set; }
    public string? LastOutput { get; private set; }
    public bool IsAuthenticated { get; private set; }
    public string? ErrorMessage { get; private set; }

    private UssdSession() { }

    public static UssdSession Create(
        string sessionId,
        string phoneNumber,
        string serviceCode,
        Guid? memberId = null,
        Guid? walletId = null,
        string? language = null)
    {
        var session = new UssdSession
        {
            SessionId = sessionId,
            PhoneNumber = phoneNumber,
            ServiceCode = serviceCode,
            MemberId = memberId,
            WalletId = walletId,
            Language = language ?? "en",
            Status = StatusActive,
            StartedAt = DateTimeOffset.UtcNow,
            LastActivityAt = DateTimeOffset.UtcNow,
            MenuLevel = 0,
            StepCount = 0
        };

        session.QueueDomainEvent(new UssdSessionStarted(session));
        return session;
    }

    public UssdSession Navigate(string input, string output, string newMenu, int newMenuLevel)
    {
        LastInput = input;
        LastOutput = output;
        CurrentMenu = newMenu;
        MenuLevel = newMenuLevel;
        StepCount++;
        LastActivityAt = DateTimeOffset.UtcNow;
        QueueDomainEvent(new UssdSessionNavigated(Id, CurrentMenu, StepCount));
        return this;
    }

    public UssdSession SetOperation(string operation, string? data = null)
    {
        CurrentOperation = operation;
        SessionData = data;
        LastActivityAt = DateTimeOffset.UtcNow;
        return this;
    }

    public UssdSession Authenticate()
    {
        IsAuthenticated = true;
        QueueDomainEvent(new UssdSessionAuthenticated(Id, PhoneNumber));
        return this;
    }

    public UssdSession Complete(string? finalOutput = null)
    {
        Status = StatusCompleted;
        EndedAt = DateTimeOffset.UtcNow;
        if (finalOutput is not null) LastOutput = finalOutput;
        QueueDomainEvent(new UssdSessionCompleted(Id, StepCount));
        return this;
    }

    public UssdSession Timeout()
    {
        Status = StatusTimedOut;
        EndedAt = DateTimeOffset.UtcNow;
        QueueDomainEvent(new UssdSessionTimedOut(Id));
        return this;
    }

    public UssdSession Cancel()
    {
        Status = StatusCancelled;
        EndedAt = DateTimeOffset.UtcNow;
        return this;
    }

    public UssdSession SetError(string errorMessage)
    {
        Status = StatusError;
        ErrorMessage = errorMessage;
        EndedAt = DateTimeOffset.UtcNow;
        return this;
    }

    public bool IsExpired()
    {
        return (DateTimeOffset.UtcNow - LastActivityAt).TotalSeconds > SessionTimeoutSeconds;
    }
}
