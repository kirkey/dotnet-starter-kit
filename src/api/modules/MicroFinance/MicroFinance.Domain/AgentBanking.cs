using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents an agent banking location for cash-in/cash-out services.
/// Enables the MFI to extend services through third-party retail agents in remote areas.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Register and manage agent banking locations</description></item>
///   <item><description>Track agent cash float and transaction volumes</description></item>
///   <item><description>Calculate and pay agent commissions</description></item>
///   <item><description>Monitor agent performance and compliance</description></item>
///   <item><description>Enable members to transact at retail locations (shops, pharmacies)</description></item>
///   <item><description>Manage agent tier levels based on volume and compliance</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// Agent banking (also known as branchless banking) is critical for financial inclusion:
/// </para>
/// <list type="bullet">
///   <item><description><strong>Reach</strong>: Serve remote areas without branch infrastructure</description></item>
///   <item><description><strong>Cost</strong>: Lower setup and operating costs than branches</description></item>
///   <item><description><strong>Convenience</strong>: Extended hours, familiar locations for members</description></item>
///   <item><description><strong>Liquidity</strong>: Agents provide cash-in/cash-out capability</description></item>
/// </list>
/// <para><strong>Agent Tiers:</strong></para>
/// <list type="bullet">
///   <item><description><strong>Bronze</strong>: New agents, lower limits, basic services</description></item>
///   <item><description><strong>Silver</strong>: Established agents, medium limits</description></item>
///   <item><description><strong>Gold</strong>: High-volume agents, higher limits</description></item>
///   <item><description><strong>Platinum</strong>: Super agents, may manage other agents</description></item>
/// </list>
/// <para><strong>Related Entities:</strong></para>
/// <list type="bullet">
///   <item><description><see cref="Branch"/> - Supervising branch</description></item>
///   <item><description><see cref="CashVault"/> - Agent float account</description></item>
///   <item><description><see cref="MobileTransaction"/> - Transactions processed by agent</description></item>
/// </list>
/// </remarks>
/// <example>
/// <para><strong>Example: Registering a new agent</strong></para>
/// <code>
/// POST /api/microfinance/agent-banking
/// {
///   "agentCode": "AGT-KGL-001",
///   "businessName": "Mama Josephine Shop",
///   "contactName": "Josephine Uwimana",
///   "phoneNumber": "+250788654321",
///   "email": "josephine@email.com",
///   "address": "Main Road, Nyamirambo, Kigali",
///   "branchId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
///   "tier": "Bronze",
///   "floatLimit": 2000000,
///   "commissionRate": 0.5
/// }
/// </code>
/// </example>
public sealed class AgentBanking : AuditableEntity, IAggregateRoot
{
    // Constants
    public const int AgentCodeMaxLength = 32;
    public const int BusinessNameMaxLength = 128;
    public const int ContactNameMaxLength = 128;
    public const int PhoneNumberMaxLength = 32;
    public const int EmailMaxLength = 128;
    public const int AddressMaxLength = 512;
    public const int StatusMaxLength = 32;
    public const int TierMaxLength = 32;
    public const int GpsMaxLength = 64;
    
    // Agent Status
    public const string StatusActive = "Active";
    public const string StatusInactive = "Inactive";
    public const string StatusSuspended = "Suspended";
    public const string StatusPendingApproval = "PendingApproval";
    public const string StatusTerminated = "Terminated";
    
    // Agent Tier
    public const string TierBronze = "Bronze";
    public const string TierSilver = "Silver";
    public const string TierGold = "Gold";
    public const string TierPlatinum = "Platinum";

    public string AgentCode { get; private set; } = default!;
    public string BusinessName { get; private set; } = default!;
    public string ContactName { get; private set; } = default!;
    public string PhoneNumber { get; private set; } = default!;
    public string? Email { get; private set; }
    public string Address { get; private set; } = default!;
    public string? GpsCoordinates { get; private set; }
    public string Status { get; private set; } = StatusPendingApproval;
    public string Tier { get; private set; } = TierBronze;
    public Guid? BranchId { get; private set; }
    public Guid? LinkedStaffId { get; private set; }
    public decimal FloatBalance { get; private set; }
    public decimal MinFloatBalance { get; private set; }
    public decimal MaxFloatBalance { get; private set; }
    public decimal CommissionRate { get; private set; }
    public decimal TotalCommissionEarned { get; private set; }
    public decimal DailyTransactionLimit { get; private set; }
    public decimal MonthlyTransactionLimit { get; private set; }
    public decimal DailyVolumeProcessed { get; private set; }
    public decimal MonthlyVolumeProcessed { get; private set; }
    public int TotalTransactionsToday { get; private set; }
    public int TotalTransactionsMonth { get; private set; }
    public DateOnly ContractStartDate { get; private set; }
    public DateOnly? ContractEndDate { get; private set; }
    public DateOnly? LastTrainingDate { get; private set; }
    public DateOnly? LastAuditDate { get; private set; }
    public bool IsKycVerified { get; private set; }
    public string? DeviceId { get; private set; }
    public string? OperatingHours { get; private set; }

    private AgentBanking() { }

    public static AgentBanking Create(
        string agentCode,
        string businessName,
        string contactName,
        string phoneNumber,
        string address,
        decimal commissionRate,
        decimal dailyTransactionLimit,
        decimal monthlyTransactionLimit,
        DateOnly contractStartDate,
        Guid? branchId = null)
    {
        var agent = new AgentBanking
        {
            AgentCode = agentCode,
            BusinessName = businessName,
            ContactName = contactName,
            PhoneNumber = phoneNumber,
            Address = address,
            CommissionRate = commissionRate,
            DailyTransactionLimit = dailyTransactionLimit,
            MonthlyTransactionLimit = monthlyTransactionLimit,
            ContractStartDate = contractStartDate,
            BranchId = branchId,
            Status = StatusPendingApproval,
            Tier = TierBronze,
            FloatBalance = 0,
            TotalCommissionEarned = 0
        };

        agent.QueueDomainEvent(new AgentBankingCreated(agent));
        return agent;
    }

    public AgentBanking Approve()
    {
        Status = StatusActive;
        QueueDomainEvent(new AgentBankingApproved(Id, AgentCode));
        return this;
    }

    public AgentBanking CreditFloat(decimal amount)
    {
        FloatBalance += amount;
        QueueDomainEvent(new AgentFloatCredited(Id, amount, FloatBalance));
        return this;
    }

    public AgentBanking DebitFloat(decimal amount)
    {
        if (amount > FloatBalance)
            throw new InvalidOperationException("Insufficient float balance.");

        FloatBalance -= amount;
        QueueDomainEvent(new AgentFloatDebited(Id, amount, FloatBalance));
        return this;
    }

    public AgentBanking RecordTransaction(decimal amount, decimal commission)
    {
        DailyVolumeProcessed += amount;
        MonthlyVolumeProcessed += amount;
        TotalTransactionsToday++;
        TotalTransactionsMonth++;
        TotalCommissionEarned += commission;
        return this;
    }

    public AgentBanking UpgradeTier(string newTier)
    {
        Tier = newTier;
        QueueDomainEvent(new AgentTierUpgraded(Id, newTier));
        return this;
    }

    public AgentBanking Suspend(string reason)
    {
        Status = StatusSuspended;
        QueueDomainEvent(new AgentBankingSuspended(Id, reason));
        return this;
    }

    public AgentBanking RecordAudit(DateOnly auditDate)
    {
        LastAuditDate = auditDate;
        return this;
    }

    public AgentBanking ResetDailyCounters()
    {
        DailyVolumeProcessed = 0;
        TotalTransactionsToday = 0;
        return this;
    }

    public AgentBanking ResetMonthlyCounters()
    {
        MonthlyVolumeProcessed = 0;
        TotalTransactionsMonth = 0;
        return this;
    }

    public AgentBanking Update(
        string? businessName = null,
        string? contactName = null,
        string? phoneNumber = null,
        string? email = null,
        string? address = null,
        string? gpsCoordinates = null,
        string? operatingHours = null,
        decimal? commissionRate = null,
        decimal? dailyTransactionLimit = null,
        decimal? monthlyTransactionLimit = null)
    {
        if (businessName is not null) BusinessName = businessName;
        if (contactName is not null) ContactName = contactName;
        if (phoneNumber is not null) PhoneNumber = phoneNumber;
        if (email is not null) Email = email;
        if (address is not null) Address = address;
        if (gpsCoordinates is not null) GpsCoordinates = gpsCoordinates;
        if (operatingHours is not null) OperatingHours = operatingHours;
        if (commissionRate.HasValue) CommissionRate = commissionRate.Value;
        if (dailyTransactionLimit.HasValue) DailyTransactionLimit = dailyTransactionLimit.Value;
        if (monthlyTransactionLimit.HasValue) MonthlyTransactionLimit = monthlyTransactionLimit.Value;

        QueueDomainEvent(new AgentBankingUpdated(this));
        return this;
    }
}
