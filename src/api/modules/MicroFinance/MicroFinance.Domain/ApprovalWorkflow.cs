using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Domain constants for ApprovalWorkflow entity.
/// </summary>
public static class ApprovalWorkflowConstants
{
    /// <summary>Maximum length for workflow code. (2^5 = 32)</summary>
    public const int CodeMaxLength = 32;

    /// <summary>Maximum length for workflow name. (2^7 = 128)</summary>
    public const int NameMaxLength = 128;

    /// <summary>Maximum length for entity type. (2^6 = 64)</summary>
    public const int EntityTypeMaxLength = 64;

    /// <summary>Maximum length for description. (2^10 = 1024)</summary>
    public const int DescriptionMaxLength = 1024;
}

/// <summary>
/// Represents a configurable multi-level approval workflow.
/// </summary>
/// <remarks>
/// Use cases:
/// - Define approval hierarchies for loans, write-offs, fee waivers.
/// - Configure approval thresholds based on amount or risk level.
/// - Set up multi-level approval chains.
/// - Enable parallel or sequential approval paths.
/// - Support regulatory compliance for credit decisions.
/// 
/// Default values and constraints:
/// - Code: Unique workflow identifier (max 32 chars).
/// - Name: Display name for the workflow (max 128 chars).
/// - EntityType: LOAN, WRITE_OFF, FEE_WAIVER, SETTLEMENT, RESTRUCTURE.
/// - Description: Workflow description (max 1024 chars).
/// - IsActive: Whether workflow is currently in use.
/// 
/// Business rules:
/// - Approval workflows ensure proper authorization for significant decisions.
/// - Different workflows for different entity types.
/// - Varying approval levels based on amount thresholds.
/// - Multi-level chains escalate larger or riskier decisions.
/// - Parallel paths allow concurrent approvals.
/// </remarks>
public class ApprovalWorkflow : AuditableEntity, IAggregateRoot
{
    // Entity Types
    /// <summary>Loan approval workflow.</summary>
    public const string EntityTypeLoan = "LOAN";
    /// <summary>Loan write-off approval workflow.</summary>
    public const string EntityTypeWriteOff = "WRITE_OFF";
    /// <summary>Fee waiver approval workflow.</summary>
    public const string EntityTypeFeeWaiver = "FEE_WAIVER";
    /// <summary>Debt settlement approval workflow.</summary>
    public const string EntityTypeSettlement = "SETTLEMENT";
    /// <summary>Loan restructuring approval workflow.</summary>
    public const string EntityTypeRestructure = "RESTRUCTURE";
    /// <summary>Expense approval workflow.</summary>
    public const string EntityTypeExpense = "EXPENSE";
    /// <summary>Disbursement approval workflow.</summary>
    public const string EntityTypeDisbursement = "DISBURSEMENT";

    /// <summary>Gets the unique workflow code.</summary>
    public string Code { get; private set; } = default!;


    /// <summary>Gets the entity type this workflow applies to.</summary>
    public string EntityType { get; private set; } = default!;

    /// <summary>Gets the minimum amount threshold for this workflow (null = any amount).</summary>
    public decimal? MinAmount { get; private set; }

    /// <summary>Gets the maximum amount threshold for this workflow (null = no limit).</summary>
    public decimal? MaxAmount { get; private set; }

    /// <summary>Gets the branch ID this workflow applies to (null = all branches).</summary>
    public DefaultIdType? BranchId { get; private set; }

    /// <summary>Gets the number of approval levels required.</summary>
    public int NumberOfLevels { get; private set; }

    /// <summary>Gets whether approvals are sequential (true) or parallel (false).</summary>
    public bool IsSequential { get; private set; }

    /// <summary>Gets whether this workflow is active.</summary>
    public bool IsActive { get; private set; }

    /// <summary>Gets the priority (lower = higher priority for matching).</summary>
    public int Priority { get; private set; }

    /// <summary>Gets the collection of approval levels.</summary>
    public virtual ICollection<ApprovalLevel> ApprovalLevels { get; private set; } = new List<ApprovalLevel>();

    private ApprovalWorkflow() { }

    private ApprovalWorkflow(
        DefaultIdType id,
        string code,
        string name,
        string entityType,
        int numberOfLevels)
    {
        Id = id;
        Code = code.Trim().ToUpperInvariant();
        Name = name.Trim();
        EntityType = entityType;
        NumberOfLevels = numberOfLevels;
        IsSequential = true;
        IsActive = true;
        Priority = 100;

        QueueDomainEvent(new ApprovalWorkflowCreated { ApprovalWorkflow = this });
    }

    /// <summary>Creates a new ApprovalWorkflow.</summary>
    public static ApprovalWorkflow Create(
        string code,
        string name,
        string entityType,
        int numberOfLevels)
    {
        return new ApprovalWorkflow(
            DefaultIdType.NewGuid(),
            code,
            name,
            entityType,
            numberOfLevels);
    }

    /// <summary>Updates the workflow.</summary>
    public ApprovalWorkflow Update(
        string? name = null,
        string? description = null,
        int? numberOfLevels = null,
        bool? isSequential = null,
        int? priority = null)
    {
        if (!string.IsNullOrWhiteSpace(name)) Name = name.Trim();
        if (description != null) Description = description.Trim();
        if (numberOfLevels.HasValue) NumberOfLevels = numberOfLevels.Value;
        if (isSequential.HasValue) IsSequential = isSequential.Value;
        if (priority.HasValue) Priority = priority.Value;
        
        return this;
    }

    /// <summary>Sets amount thresholds.</summary>
    public ApprovalWorkflow WithAmountThresholds(decimal? minAmount, decimal? maxAmount)
    {
        MinAmount = minAmount;
        MaxAmount = maxAmount;
        return this;
    }

    /// <summary>Associates with a specific branch.</summary>
    public ApprovalWorkflow ForBranch(DefaultIdType branchId)
    {
        BranchId = branchId;
        return this;
    }

    /// <summary>Activates the workflow.</summary>
    public ApprovalWorkflow Activate()
    {
        IsActive = true;
        return this;
    }

    /// <summary>Deactivates the workflow.</summary>
    public ApprovalWorkflow Deactivate()
    {
        IsActive = false;
        return this;
    }

    /// <summary>Checks if this workflow applies to the given entity and amount.</summary>
    public bool AppliesTo(string entityType, decimal? amount, DefaultIdType? branchId)
    {
        if (!IsActive) return false;
        if (EntityType != entityType) return false;
        if (BranchId.HasValue && BranchId != branchId) return false;
        if (amount.HasValue)
        {
            if (MinAmount.HasValue && amount < MinAmount) return false;
            if (MaxAmount.HasValue && amount > MaxAmount) return false;
        }
        return true;
    }
}

/// <summary>
/// Represents an approval level within a workflow.
/// </summary>
public class ApprovalLevel : AuditableEntity
{
    /// <summary>Gets the workflow ID.</summary>
    public DefaultIdType WorkflowId { get; private set; }

    /// <summary>Gets the workflow navigation property.</summary>
    public virtual ApprovalWorkflow? Workflow { get; private set; }

    /// <summary>Gets the level number (1 = first approver).</summary>
    public int LevelNumber { get; private set; }

    /// <summary>Gets the role required for this level (null = any authorized user).</summary>
    public string? RequiredRole { get; private set; }

    /// <summary>Gets the specific user ID required (null = any user with role).</summary>
    public DefaultIdType? RequiredUserId { get; private set; }

    /// <summary>Gets the maximum amount this level can approve.</summary>
    public decimal? MaxApprovalAmount { get; private set; }

    /// <summary>Gets whether this level can be skipped.</summary>
    public bool CanBeSkipped { get; private set; }

    /// <summary>Gets the SLA hours for approval.</summary>
    public int? SlaHours { get; private set; }

    private ApprovalLevel() { }

    public static ApprovalLevel Create(
        DefaultIdType workflowId,
        int levelNumber,
        string? requiredRole = null,
        DefaultIdType? requiredUserId = null)
    {
        return new ApprovalLevel
        {
            Id = DefaultIdType.NewGuid(),
            WorkflowId = workflowId,
            LevelNumber = levelNumber,
            RequiredRole = requiredRole?.Trim(),
            RequiredUserId = requiredUserId,
            CanBeSkipped = false
        };
    }

    public ApprovalLevel WithMaxAmount(decimal maxAmount)
    {
        MaxApprovalAmount = maxAmount;
        return this;
    }

    public ApprovalLevel WithSla(int hours)
    {
        SlaHours = hours;
        return this;
    }

    public ApprovalLevel SetSkippable(bool canBeSkipped)
    {
        CanBeSkipped = canBeSkipped;
        return this;
    }
}
