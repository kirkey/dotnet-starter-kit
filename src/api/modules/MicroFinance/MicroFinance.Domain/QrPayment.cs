using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a QR code for payments.
/// </summary>
public sealed class QrPayment : AuditableEntity, IAggregateRoot
{
    // Constants
    public const int QrCodeMaxLength = 512;
    public const int TypeMaxLength = 32;
    public const int StatusMaxLength = 32;
    public const int ReferenceMaxLength = 64;
    public const int NotesMaxLength = 256;
    
    // QR Payment Type
    public const string TypeStatic = "Static";
    public const string TypeDynamic = "Dynamic";
    public const string TypeMerchant = "Merchant";
    public const string TypeP2P = "P2P";
    
    // QR Payment Status
    public const string StatusActive = "Active";
    public const string StatusUsed = "Used";
    public const string StatusExpired = "Expired";
    public const string StatusCancelled = "Cancelled";

    public Guid? WalletId { get; private set; }
    public Guid? MemberId { get; private set; }
    public Guid? AgentId { get; private set; }
    public string QrCode { get; private set; } = default!;
    public string QrType { get; private set; } = default!;
    public string Status { get; private set; } = StatusActive;
    public decimal? Amount { get; private set; }
    public string? Reference { get; private set; }
    public int MaxUses { get; private set; } = 1;
    public int CurrentUses { get; private set; }
    public DateTimeOffset GeneratedAt { get; private set; }
    public DateTimeOffset? ExpiresAt { get; private set; }
    public DateTimeOffset? LastUsedAt { get; private set; }
    public Guid? LastTransactionId { get; private set; }

    private QrPayment() { }

    public static QrPayment CreateStatic(
        string qrCode,
        Guid? walletId = null,
        Guid? memberId = null,
        Guid? agentId = null)
    {
        var qr = new QrPayment
        {
            QrCode = qrCode,
            QrType = TypeStatic,
            WalletId = walletId,
            MemberId = memberId,
            AgentId = agentId,
            Status = StatusActive,
            MaxUses = int.MaxValue,
            GeneratedAt = DateTimeOffset.UtcNow
        };

        qr.QueueDomainEvent(new QrPaymentCreated(qr));
        return qr;
    }

    public static QrPayment CreateDynamic(
        string qrCode,
        decimal amount,
        string reference,
        DateTimeOffset expiresAt,
        Guid? walletId = null,
        Guid? memberId = null)
    {
        var qr = new QrPayment
        {
            QrCode = qrCode,
            QrType = TypeDynamic,
            Amount = amount,
            Reference = reference,
            WalletId = walletId,
            MemberId = memberId,
            Status = StatusActive,
            MaxUses = 1,
            GeneratedAt = DateTimeOffset.UtcNow,
            ExpiresAt = expiresAt
        };

        qr.QueueDomainEvent(new QrPaymentCreated(qr));
        return qr;
    }

    public QrPayment Use(Guid transactionId)
    {
        if (Status != StatusActive)
            throw new InvalidOperationException("QR code is not active.");

        if (ExpiresAt.HasValue && DateTimeOffset.UtcNow > ExpiresAt)
        {
            Status = StatusExpired;
            throw new InvalidOperationException("QR code has expired.");
        }

        CurrentUses++;
        LastUsedAt = DateTimeOffset.UtcNow;
        LastTransactionId = transactionId;

        if (CurrentUses >= MaxUses)
        {
            Status = StatusUsed;
        }

        QueueDomainEvent(new QrPaymentUsed(Id, transactionId));
        return this;
    }

    public QrPayment Cancel()
    {
        Status = StatusCancelled;
        return this;
    }

    public QrPayment Expire()
    {
        Status = StatusExpired;
        return this;
    }

    public bool IsValid()
    {
        if (Status != StatusActive) return false;
        if (ExpiresAt.HasValue && DateTimeOffset.UtcNow > ExpiresAt) return false;
        if (CurrentUses >= MaxUses) return false;
        return true;
    }
}
