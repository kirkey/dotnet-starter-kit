namespace Accounting.Domain;

public class SecurityDeposit : AuditableEntity, IAggregateRoot
{
    public DefaultIdType MemberId { get; private set; }
    public decimal DepositAmount { get; private set; }
    public DateTime DepositDate { get; private set; }
    public bool IsRefunded { get; private set; }
    public DateTime? RefundedDate { get; private set; }
    public string? RefundReference { get; private set; }

    private SecurityDeposit()
    {
        MemberId = DefaultIdType.Empty;
        DepositAmount = 0m;
        DepositDate = DateTime.MinValue;
        IsRefunded = false;
    }

    private SecurityDeposit(DefaultIdType memberId, decimal depositAmount, DateTime depositDate, string? description = null, string? notes = null)
    {
        MemberId = memberId;
        DepositAmount = depositAmount;
        DepositDate = depositDate;
        IsRefunded = false;
        Description = description?.Trim();
        Notes = notes?.Trim();
    }

    public static SecurityDeposit Create(DefaultIdType memberId, decimal depositAmount, DateTime depositDate, string? description = null, string? notes = null)
    {
        if (depositAmount <= 0) throw new ArgumentException("Deposit amount must be positive.");
        return new SecurityDeposit(memberId, depositAmount, depositDate, description, notes);
    }

    public SecurityDeposit Refund(DateTime refundedDate, string? refundReference = null)
    {
        if (IsRefunded) throw new InvalidOperationException("Deposit already refunded.");
        IsRefunded = true;
        RefundedDate = refundedDate;
        RefundReference = refundReference?.Trim();
        return this;
    }
}
