using Accounting.Domain.Events.ProjectCosting;

namespace Accounting.Domain;

/// <summary>
/// Represents a cost entry associated with a project for job costing and budget tracking.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track individual cost entries for materials, labor, equipment, and overhead expenses
/// - Support job costing with detailed cost categorization and allocation
/// - Enable cost variance analysis between actual and budgeted amounts
/// - Provide audit trail for all project-related expenses
/// - Support cost center allocation and departmental charging
/// - Enable time and material billing integration
/// - Track billable vs non-billable costs for client invoicing
/// - Support regulatory reporting for grant-funded projects
///
/// Default values:
/// - EntryDate: required date when cost was incurred (example: 2025-09-15)
/// - Amount: required cost amount, must be positive (example: 1250.75)
/// - Description: required description of the cost (example: "Materials for electrical work")
/// - Category: optional cost category (example: "Materials", "Labor", "Equipment")
/// - IsBillable: false (whether cost can be billed to client)
/// - IsApproved: false (whether cost entry has been approved)
/// - CostCenter: optional cost center code (example: "CC001")
/// - WorkOrderNumber: optional work order reference
///
/// Business rules:
/// - Amount must be positive (costs only, revenues handled separately)
/// - EntryDate cannot be in future beyond current fiscal period
/// - Cannot modify approved cost entries without special authorization
/// - Cost category must be valid for the associated project type
/// - Cost center must exist if specified
/// - Account ID must reference a valid expense account
/// - Description must be meaningful and not just account name
/// - Billable costs require client billing authorization
/// </remarks>
/// <seealso cref="ProjectCostEntryCreated"/>
/// <seealso cref="ProjectCostEntryUpdated"/>
/// <seealso cref="ProjectCostEntryApproved"/>
/// <seealso cref="ProjectCostEntryDeleted"/>
public class ProjectCostEntry : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Reference to the associated project.
    /// </summary>
    public DefaultIdType ProjectId { get; private set; }

    /// <summary>
    /// Date when the cost was incurred.
    /// </summary>
    public DateTime EntryDate { get; private set; }

    /// <summary>
    /// Cost amount; must be positive.
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Optional cost category for classification.
    /// </summary>
    public string? Category { get; private set; }

    /// <summary>
    /// Reference to the chart of accounts entry.
    /// </summary>
    public DefaultIdType AccountId { get; private set; }

    /// <summary>
    /// Optional reference to journal entry.
    /// </summary>
    public DefaultIdType? JournalEntryId { get; private set; }

    /// <summary>
    /// Optional cost center for departmental allocation.
    /// </summary>
    public string? CostCenter { get; private set; }

    /// <summary>
    /// Optional work order number reference.
    /// </summary>
    public string? WorkOrderNumber { get; private set; }

    /// <summary>
    /// Whether this cost can be billed to client.
    /// </summary>
    public bool IsBillable { get; private set; }

    /// <summary>
    /// Whether this cost entry has been approved.
    /// </summary>
    public bool IsApproved { get; private set; }

    /// <summary>
    /// Optional vendor or supplier reference.
    /// </summary>
    public string? Vendor { get; private set; }

    /// <summary>
    /// Optional invoice or receipt number.
    /// </summary>
    public string? InvoiceNumber { get; private set; }

    // EF parameterless constructor for EF Core instantiation
    protected ProjectCostEntry() { }

    private ProjectCostEntry(DefaultIdType projectId, DateTime entryDate, decimal amount, string description,
        DefaultIdType accountId, string? category = null, DefaultIdType? journalEntryId = null,
        string? costCenter = null, string? workOrderNumber = null, bool isBillable = false,
        string? vendor = null, string? invoiceNumber = null)
    {
        ProjectId = projectId;
        EntryDate = entryDate;
        Amount = amount;
        Description = description.Trim();
        AccountId = accountId;
        Category = category?.Trim();
        JournalEntryId = journalEntryId;
        CostCenter = costCenter?.Trim();
        WorkOrderNumber = workOrderNumber?.Trim();
        IsBillable = isBillable;
        IsApproved = false;
        Vendor = vendor?.Trim();
        InvoiceNumber = invoiceNumber?.Trim();

        QueueDomainEvent(new ProjectCostEntryCreated(Id, ProjectId, Amount, Description, Category));
    }

    /// <summary>
    /// Create a new project cost entry with validation.
    /// </summary>
    public static ProjectCostEntry Create(DefaultIdType projectId, DateTime entryDate, decimal amount, string description,
        DefaultIdType accountId, string? category = null, DefaultIdType? journalEntryId = null,
        string? costCenter = null, string? workOrderNumber = null, bool isBillable = false,
        string? vendor = null, string? invoiceNumber = null)
    {
        if (amount <= 0)
            throw new InvalidProjectCostAmountException();

        if (string.IsNullOrWhiteSpace(description))
            throw new ProjectCostDescriptionRequiredException();

        if (entryDate > DateTime.UtcNow.Date)
            throw new InvalidProjectCostDateException();

        return new ProjectCostEntry(projectId, entryDate, amount, description, accountId, category,
            journalEntryId, costCenter, workOrderNumber, isBillable, vendor, invoiceNumber);
    }

    /// <summary>
    /// Update project cost entry details with validation.
    /// </summary>
    public ProjectCostEntry Update(DateTime? entryDate = null, decimal? amount = null, string? description = null,
        string? category = null, string? costCenter = null, string? workOrderNumber = null,
        bool? isBillable = null, string? vendor = null, string? invoiceNumber = null)
    {
        if (IsApproved)
            throw new ApprovedProjectCostCannotBeModifiedException(Id);

        bool isUpdated = false;

        if (entryDate.HasValue && EntryDate != entryDate.Value)
        {
            if (entryDate.Value > DateTime.UtcNow.Date)
                throw new InvalidProjectCostDateException();
            EntryDate = entryDate.Value;
            isUpdated = true;
        }

        if (amount.HasValue && Amount != amount.Value)
        {
            if (amount.Value <= 0)
                throw new InvalidProjectCostAmountException();
            Amount = amount.Value;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(description) && Description != description)
        {
            Description = description.Trim();
            isUpdated = true;
        }

        if (category != Category)
        {
            Category = category?.Trim();
            isUpdated = true;
        }

        if (costCenter != CostCenter)
        {
            CostCenter = costCenter?.Trim();
            isUpdated = true;
        }

        if (workOrderNumber != WorkOrderNumber)
        {
            WorkOrderNumber = workOrderNumber?.Trim();
            isUpdated = true;
        }

        if (isBillable.HasValue && IsBillable != isBillable.Value)
        {
            IsBillable = isBillable.Value;
            isUpdated = true;
        }

        if (vendor != Vendor)
        {
            Vendor = vendor?.Trim();
            isUpdated = true;
        }

        if (invoiceNumber != InvoiceNumber)
        {
            InvoiceNumber = invoiceNumber?.Trim();
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new ProjectCostEntryUpdated(Id));
        }

        return this;
    }

    /// <summary>
    /// Approve the project cost entry.
    /// </summary>
    public ProjectCostEntry Approve()
    {
        if (IsApproved)
            return this;

        IsApproved = true;
        QueueDomainEvent(new ProjectCostEntryApproved(Id, ProjectId, Amount));
        return this;
    }

    /// <summary>
    /// Mark the project cost entry for deletion (soft delete).
    /// </summary>
    public ProjectCostEntry MarkForDeletion()
    {
        if (IsApproved)
            throw new ApprovedProjectCostCannotBeModifiedException(Id);

        QueueDomainEvent(new ProjectCostEntryDeleted(Id, ProjectId, Amount));
        return this;
    }
}
