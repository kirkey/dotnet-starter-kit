using Accounting.Domain.Events.Project;

namespace Accounting.Domain;

public class Project : AuditableEntity, IAggregateRoot
{
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public decimal BudgetedAmount { get; private set; }
    public string Status { get; private set; } // Active, Completed, On Hold, Canceled
    public string? ClientName { get; private set; }
    public string? ProjectManager { get; private set; }
    public string? Department { get; private set; }
    public decimal ActualCost { get; private set; }
    public decimal ActualRevenue { get; private set; }
    
    private Project()
    {
        // EF Core requires a parameterless constructor for entity instantiation
    }

    private readonly List<JobCostingEntry> _costingEntries = new();
    public IReadOnlyCollection<JobCostingEntry> CostingEntries => _costingEntries.AsReadOnly();

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

    public static Project Create(string projectName, DateTime startDate, decimal budgetedAmount,
        string? clientName = null, string? projectManager = null, string? department = null,
        string? description = null, string? notes = null)
    {
        if (budgetedAmount < 0)
            throw new InvalidProjectBudgetException();

        return new Project(projectName, startDate, budgetedAmount, clientName, projectManager, department, description, notes);
    }

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

    public Project AddCostEntry(DateTime date, string description, decimal amount, DefaultIdType expenseAccountId,
        DefaultIdType? journalEntryId = null, string? category = null)
    {
        if (Status == "Completed" || Status == "Cancelled")
            throw new ProjectCannotBeModifiedException(Id);

        if (amount <= 0)
            throw new InvalidProjectCostEntryException();

        var entry = JobCostingEntry.Create(Id, date, description, amount, expenseAccountId, journalEntryId, category);
        _costingEntries.Add(entry);

        ActualCost += amount;
        QueueDomainEvent(new ProjectCostAdded(Id, amount, ActualCost));
        return this;
    }

    public Project AddRevenueEntry(DateTime date, string description, decimal amount, DefaultIdType revenueAccountId,
        DefaultIdType? journalEntryId = null)
    {
        if (Status == "Completed" || Status == "Cancelled")
            throw new ProjectCannotBeModifiedException(Id);

        if (amount <= 0)
            throw new InvalidProjectRevenueEntryException();

        var entry = JobCostingEntry.Create(Id, date, description, -amount, revenueAccountId, journalEntryId, "Revenue");
        _costingEntries.Add(entry);

        ActualRevenue += amount;
        QueueDomainEvent(new ProjectRevenueAdded(Id, amount, ActualRevenue));
        return this;
    }

    public Project Complete(DateTime completionDate)
    {
        if (Status == "Completed")
            throw new ProjectAlreadyCompletedException(Id);

        Status = "Completed";
        EndDate = completionDate;
        QueueDomainEvent(new ProjectCompleted(Id, completionDate, ActualCost, ActualRevenue));
        return this;
    }

    public Project Cancel(DateTime cancellationDate, string reason)
    {
        if (Status == "Completed" || Status == "Cancelled")
            throw new ProjectAlreadyCancelledException(Id);

        Status = "Cancelled";
        EndDate = cancellationDate;
        QueueDomainEvent(new ProjectCancelled(Id, cancellationDate, reason));
        return this;
    }

    public decimal GetBudgetVariance()
    {
        return BudgetedAmount - ActualCost;
    }

    public decimal GetProfitLoss()
    {
        return ActualRevenue - ActualCost;
    }

    public decimal GetBudgetUtilizationPercentage()
    {
        return BudgetedAmount > 0 ? (ActualCost / BudgetedAmount) * 100 : 0;
    }
}

public class JobCostingEntry : BaseEntity
{
    public DefaultIdType ProjectId { get; private set; }
    public DateTime Date { get; private set; }
    public string Description { get; private set; }
    public decimal Amount { get; private set; }
    public DefaultIdType AccountId { get; private set; }
    public DefaultIdType? JournalEntryId { get; private set; }
    public string? Category { get; private set; }

    private JobCostingEntry(DefaultIdType projectId, DateTime date, string description,
        decimal amount, DefaultIdType accountId, DefaultIdType? journalEntryId = null, string? category = null)
    {
        ProjectId = projectId;
        Date = date;
        Description = description.Trim();
        Amount = amount;
        AccountId = accountId;
        JournalEntryId = journalEntryId;
        Category = category?.Trim();
    }

    public static JobCostingEntry Create(DefaultIdType projectId, DateTime date, string description,
        decimal amount, DefaultIdType accountId, DefaultIdType? journalEntryId = null, string? category = null)
    {
        return new JobCostingEntry(projectId, date, description, amount, accountId, journalEntryId, category);
    }

    public JobCostingEntry Update(DateTime? date, string? description, decimal? amount, string? category)
    {
        if (date.HasValue && Date != date.Value)
        {
            Date = date.Value;
        }

        if (!string.IsNullOrWhiteSpace(description) && Description != description)
        {
            Description = description.Trim();
        }

        if (amount.HasValue && Amount != amount.Value)
        {
            Amount = amount.Value;
        }

        if (category != Category)
        {
            Category = category?.Trim();
        }

        return this;
    }
}
