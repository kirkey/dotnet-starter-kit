using Accounting.Domain.Events.Project;

namespace Accounting.Domain;

/// <summary>
/// Represents a project for job costing, budget tracking, and financial performance analysis across multiple cost centers.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track project costs and revenues for construction, maintenance, and capital improvement projects.
/// - Support job costing with actual vs. budgeted cost analysis and variance reporting.
/// - Enable project-based financial reporting for management and regulatory compliance.
/// - Manage project lifecycle from initiation through completion with status tracking.
/// - Support work order integration and labor/material cost allocation to projects.
/// - Track project profitability and performance metrics for future planning.
/// - Enable project budget control with spend authorization and approval workflows.
/// - Support grant-funded projects with specific cost tracking and reporting requirements.
///
/// Default values:
/// - StartDate: required project initiation date (example: 2025-09-01)
/// - EndDate: null (set when project is completed or cancelled)
/// - BudgetedAmount: required approved budget (example: 125000.00 for construction project)
/// - Status: "Active" (new projects start as active)
/// - ActualCost: 0.00 (accumulated as costs are posted to project)
/// - ActualRevenue: 0.00 (accumulated as revenues are recognized)
/// - Client: optional client or department name (example: "Engineering Department")
/// - ProjectManager: optional manager assignment (example: "John Smith")
/// - Name: inherited project name (example: "Substation Alpha Upgrade")
/// - Description: inherited project description (example: "Replace aging substation equipment")
///
/// Business rules:
/// - BudgetedAmount must be non-negative
/// - EndDate must be after StartDate when specified
/// - Cannot change status to Completed without setting EndDate
/// - ActualCost cannot exceed approved budget without authorization
/// - Cannot post costs to Cancelled or Completed projects
/// - Project codes must be unique within the organization
/// - Status changes require proper approval workflows
/// - Budget changes require documented approval process
/// </remarks>
/// <seealso cref="Accounting.Domain.Events.Project.ProjectCreated"/>
/// <seealso cref="Accounting.Domain.Events.Project.ProjectUpdated"/>
/// <seealso cref="Accounting.Domain.Events.Project.ProjectCompleted"/>
/// <seealso cref="Accounting.Domain.Events.Project.ProjectCancelled"/>
/// <seealso cref="Accounting.Domain.Events.Project.ProjectBudgetAdjusted"/>
/// <seealso cref="Accounting.Domain.Events.Project.ProjectCostPosted"/>
/// <seealso cref="Accounting.Domain.Events.Project.ProjectRevenueRecognized"/>
public class Project : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Project start date.
    /// </summary>
    public DateTime StartDate { get; private set; }

    /// <summary>
    /// Project completion/cancellation date, when applicable.
    /// </summary>
    public DateTime? EndDate { get; private set; }

    /// <summary>
    /// Approved budget amount for the project; must be non-negative.
    /// </summary>
    public decimal BudgetedAmount { get; private set; }

    /// <summary>
    /// Status: Active, Completed, On Hold, or Cancelled.
    /// </summary>
    public string Status { get; private set; } // Active, Completed, On Hold, Canceled

    /// <summary>
    /// Optional end-customer/client name.
    /// </summary>
    public string? ClientName { get; private set; }

    /// <summary>
    /// Optional project manager name.
    /// </summary>
    public string? ProjectManager { get; private set; }

    /// <summary>
    /// Owning department.
    /// </summary>
    public string? Department { get; private set; }

    /// <summary>
    /// Accumulated actual costs from job costing entries (positive amounts).
    /// </summary>
    public decimal ActualCost { get; private set; }

    /// <summary>
    /// Accumulated actual revenues (sum of negative amount entries categorized as revenue).
    /// </summary>
    public decimal ActualRevenue { get; private set; }
    
    private Project()
    {
        // EF Core requires a parameterless constructor for entity instantiation
    }

    private readonly List<ProjectCostEntry> _costingEntries = new();
    /// <summary>
    /// Cost and revenue entries associated with this project.
    /// </summary>
    public IReadOnlyCollection<ProjectCostEntry> CostingEntries => _costingEntries.AsReadOnly();

    private Project(string projectName, DateTime startDate, decimal budgetedAmount,
        string? clientName = null, string? projectManager = null, string? department = null,
        string? description = null, string? notes = null)
    {
        Name = projectName.Trim();
        StartDate = startDate;
        BudgetedAmount = budgetedAmount;
        Status = "Active";
        ClientName = clientName?.Trim();
        ProjectManager = projectManager?.Trim();
        Department = department?.Trim();
        ActualCost = 0;
        ActualRevenue = 0;
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new ProjectCreated(Id, Name, StartDate, BudgetedAmount, Description, Notes));
    }

    /// <summary>
    /// Create a project; budget must be non-negative.
    /// </summary>
    public static Project Create(string projectName, DateTime startDate, decimal budgetedAmount,
        string? clientName = null, string? projectManager = null, string? department = null,
        string? description = null, string? notes = null)
    {
        if (budgetedAmount < 0)
            throw new InvalidProjectBudgetException();

        return new Project(projectName, startDate, budgetedAmount, clientName, projectManager, department, description, notes);
    }

    /// <summary>
    /// Update project metadata and figures; validates non-negative budgets.
    /// </summary>
    public Project Update(string? projectName, DateTime? startDate, DateTime? endDate, decimal? budgetedAmount,
        string? status, string? clientName, string? projectManager, string? department,
        string? description, string? notes)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(projectName) && Name != projectName)
        {
            Name = projectName.Trim();
            isUpdated = true;
        }

        if (startDate.HasValue && StartDate != startDate.Value)
        {
            StartDate = startDate.Value;
            isUpdated = true;
        }

        if (endDate != EndDate)
        {
            EndDate = endDate;
            isUpdated = true;
        }

        if (budgetedAmount.HasValue && BudgetedAmount != budgetedAmount.Value)
        {
            if (budgetedAmount.Value < 0)
                throw new InvalidProjectBudgetException();
            BudgetedAmount = budgetedAmount.Value;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(status) && Status != status)
        {
            Status = status.Trim();
            isUpdated = true;
        }

        if (clientName != ClientName)
        {
            ClientName = clientName?.Trim();
            isUpdated = true;
        }

        if (projectManager != ProjectManager)
        {
            ProjectManager = projectManager?.Trim();
            isUpdated = true;
        }

        if (department != Department)
        {
            Department = department?.Trim();
            isUpdated = true;
        }

        if (description != Description)
        {
            Description = description?.Trim();
            isUpdated = true;
        }

        if (notes != Notes)
        {
            Notes = notes?.Trim();
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new ProjectUpdated(this));
        }

        return this;
    }

    /// <summary>
    /// Add a cost entry; amount must be positive. Not allowed when project is Completed or Cancelled.
    /// </summary>
    public Project AddCostEntry(DateTime date, string description, decimal amount, DefaultIdType expenseAccountId,
        DefaultIdType? journalEntryId = null, string? category = null)
    {
        if (Status == "Completed" || Status == "Cancelled")
            throw new ProjectCannotBeModifiedException(Id);

        if (amount <= 0)
            throw new InvalidProjectCostEntryException();

        var entry = ProjectCostEntry.Create(Id, date, description, amount, expenseAccountId, journalEntryId, category);
        _costingEntries.Add(entry);

        ActualCost += amount;
        QueueDomainEvent(new ProjectCostAdded(Id, amount, ActualCost));
        return this;
    }

    /// <summary>
    /// Add a revenue entry; amount must be positive. Not allowed when project is Completed or Cancelled.
    /// </summary>
    public Project AddRevenueEntry(DateTime date, string description, decimal amount, DefaultIdType revenueAccountId,
        DefaultIdType? journalEntryId = null)
    {
        if (Status == "Completed" || Status == "Cancelled")
            throw new ProjectCannotBeModifiedException(Id);

        if (amount <= 0)
            throw new InvalidProjectRevenueEntryException();

        var entry = ProjectCostEntry.Create(Id, date, description, -amount, revenueAccountId, journalEntryId, "Revenue");
        _costingEntries.Add(entry);

        ActualRevenue += amount;
        QueueDomainEvent(new ProjectRevenueAdded(Id, amount, ActualRevenue));
        return this;
    }

    /// <summary>
    /// Mark project as Completed and set <see cref="EndDate"/>.
    /// </summary>
    public Project Complete(DateTime completionDate)
    {
        if (Status == "Completed")
            throw new ProjectAlreadyCompletedException(Id);

        Status = "Completed";
        EndDate = completionDate;
        QueueDomainEvent(new ProjectCompleted(Id, completionDate, ActualCost, ActualRevenue));
        return this;
    }

    /// <summary>
    /// Cancel project and set <see cref="EndDate"/> with a reason (logged in event).
    /// </summary>
    public Project Cancel(DateTime cancellationDate, string reason)
    {
        if (Status == "Completed" || Status == "Cancelled")
            throw new ProjectAlreadyCancelledException(Id);

        Status = "Cancelled";
        EndDate = cancellationDate;
        QueueDomainEvent(new ProjectCancelled(Id, cancellationDate, reason));
        return this;
    }

    /// <summary>
    /// Difference between budget and actual cost (positive = under budget).
    /// </summary>
    public decimal GetBudgetVariance()
    {
        return BudgetedAmount - ActualCost;
    }

    /// <summary>
    /// Profit/Loss calculated as ActualRevenue - ActualCost.
    /// </summary>
    public decimal GetProfitLoss()
    {
        return ActualRevenue - ActualCost;
    }

    /// <summary>
    /// Percentage of budget consumed by actual costs.
    /// </summary>
    public decimal GetBudgetUtilizationPercentage()
    {
        return BudgetedAmount > 0 ? (ActualCost / BudgetedAmount) * 100 : 0;
    }

    /// <summary>
    /// Update an existing cost entry and adjust <see cref="ActualCost"/> by the delta.
    /// Only positive-amount cost entries are supported by this operation.
    /// </summary>
    /// <param name="entryId">The identifier of the job costing entry to update.</param>
    /// <param name="date">Optional new date.</param>
    /// <param name="description">Optional new description.</param>
    /// <param name="amount">Optional new amount (must be &gt; 0 when provided).</param>
    /// <param name="category">Optional new category.</param>
    /// <exception cref="ProjectCannotBeModifiedException">Thrown when project is Completed or Cancelled.</exception>
    /// <exception cref="JobCostingEntryNotFoundException">Thrown when the entry is not found.</exception>
    /// <exception cref="InvalidProjectCostEntryException">Thrown when provided amount is invalid.</exception>
    public Project UpdateCostEntry(DefaultIdType entryId, DateTime? date, string? description, decimal? amount, string? category)
    {
        if (Status == "Completed" || Status == "Cancelled")
            throw new ProjectCannotBeModifiedException(Id);

        var entry = _costingEntries.FirstOrDefault(e => e.Id == entryId)
                    ?? throw new JobCostingEntryNotFoundException(entryId);

        var oldAmount = entry.Amount;

        if (amount.HasValue && amount.Value <= 0)
            throw new InvalidProjectCostEntryException();

        // Only allow update through this method for cost entries (positive amounts)
        if (oldAmount <= 0)
            throw new InvalidProjectCostEntryException();

        entry.Update(date, description, amount, category);

        if (amount.HasValue && amount.Value != oldAmount)
        {
            var delta = amount.Value - oldAmount;
            ActualCost += delta;
        }

        return this;
    }

    /// <summary>
    /// Remove an existing cost entry and decrease <see cref="ActualCost"/> accordingly.
    /// This operation supports only positive-amount cost entries.
    /// </summary>
    /// <param name="entryId">The identifier of the job costing entry to remove.</param>
    /// <exception cref="ProjectCannotBeModifiedException">Thrown when project is Completed or Cancelled.</exception>
    /// <exception cref="JobCostingEntryNotFoundException">Thrown when the entry is not found.</exception>
    public Project RemoveCostEntry(DefaultIdType entryId)
    {
        if (Status == "Completed" || Status == "Cancelled")
            throw new ProjectCannotBeModifiedException(Id);

        var entry = _costingEntries.FirstOrDefault(e => e.Id == entryId)
                    ?? throw new JobCostingEntryNotFoundException(entryId);

        // Only allow removal through this method for cost entries (positive amounts)
        if (entry.Amount <= 0)
            throw new InvalidProjectCostEntryException();

        ActualCost -= entry.Amount;
        _costingEntries.Remove(entry);
        return this;
    }
}
