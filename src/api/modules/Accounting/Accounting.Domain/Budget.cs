using Accounting.Domain.Events.Budget;
using Accounting.Domain.Exceptions;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace Accounting.Domain;

public class Budget : AuditableEntity, IAggregateRoot
{
    private const int MaxNameLength = 256;
    private const int MaxBudgetTypeLength = 32;
    private const int MaxStatusLength = 16;
    private const int MaxDescriptionLength = 1000;
    private const int MaxNotesLength = 1000;

    public DefaultIdType PeriodId { get; private set; }
    public int FiscalYear { get; private set; }
    public string BudgetType { get; private set; } = string.Empty; // Operating, Capital, Cash Flow
    public string Status { get; private set; } = string.Empty; // Draft, Approved, Active, Closed
    public decimal TotalBudgetedAmount { get; private set; }
    public decimal TotalActualAmount { get; private set; }
    public DateTime? ApprovedDate { get; private set; }
    public string? ApprovedBy { get; private set; }

    private readonly List<BudgetLine> _budgetLines = new();
    public IReadOnlyCollection<BudgetLine> BudgetLines => _budgetLines.AsReadOnly();

    // Parameterless constructor for EF Core
    private Budget()
    {
    }

    private Budget(string budgetName, DefaultIdType periodId, int fiscalYear, string budgetType, string? description = null, string? notes = null)
    {
        var name = (budgetName ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Budget name is required.");
        if (name.Length > MaxNameLength)
            throw new ArgumentException($"Budget name cannot exceed {MaxNameLength} characters.");

        if (periodId == default)
            throw new ArgumentException("PeriodId is required.");

        if (fiscalYear < 1900 || fiscalYear > 2100)
            throw new ArgumentException("Fiscal year is out of range.");

        var bt = (budgetType ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(bt))
            throw new ArgumentException("BudgetType is required.");
        if (bt.Length > MaxBudgetTypeLength)
            throw new ArgumentException($"BudgetType cannot exceed {MaxBudgetTypeLength} characters.");

        Name = name;
        PeriodId = periodId;
        FiscalYear = fiscalYear;
        BudgetType = bt;
        Status = "Draft";
        TotalBudgetedAmount = 0;
        TotalActualAmount = 0;

        var desc = description?.Trim();
        if (!string.IsNullOrEmpty(desc) && desc.Length > MaxDescriptionLength)
            desc = desc.Substring(0, MaxDescriptionLength);
        Description = desc;

        var nts = notes?.Trim();
        if (!string.IsNullOrEmpty(nts) && nts.Length > MaxNotesLength)
            nts = nts.Substring(0, MaxNotesLength);
        Notes = nts;

        QueueDomainEvent(new BudgetCreated(Id, Name, PeriodId, FiscalYear, BudgetType, Description, Notes));
    }

    public static Budget Create(string budgetName, DefaultIdType periodId, int fiscalYear, string budgetType, string? description = null, string? notes = null)
    {
        return new Budget(budgetName, periodId, fiscalYear, budgetType, description, notes);
    }

    public Budget Update(string? budgetName, string? budgetType, string? status, string? description, string? notes)
    {
        bool isUpdated = false;

        if (Status == "Approved" || Status == "Active")
            throw new BudgetCannotBeModifiedException(Id);

        if (!string.IsNullOrWhiteSpace(budgetName) && Name != budgetName)
        {
            var n = budgetName.Trim();
            if (n.Length > MaxNameLength)
                throw new ArgumentException($"Budget name cannot exceed {MaxNameLength} characters.");
            Name = n;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(budgetType) && BudgetType != budgetType)
        {
            var bt = budgetType.Trim();
            if (bt.Length > MaxBudgetTypeLength)
                throw new ArgumentException($"BudgetType cannot exceed {MaxBudgetTypeLength} characters.");
            BudgetType = bt;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(status) && Status != status)
        {
            var st = status.Trim();
            if (st.Length > MaxStatusLength)
                throw new ArgumentException($"Status cannot exceed {MaxStatusLength} characters.");
            Status = st;
            isUpdated = true;
        }

        if (description != Description)
        {
            var desc = description?.Trim();
            if (!string.IsNullOrEmpty(desc) && desc.Length > MaxDescriptionLength)
                desc = desc.Substring(0, MaxDescriptionLength);
            Description = desc;
            isUpdated = true;
        }

        if (notes != Notes)
        {
            var nts = notes?.Trim();
            if (!string.IsNullOrEmpty(nts) && nts.Length > MaxNotesLength)
                nts = nts.Substring(0, MaxNotesLength);
            Notes = nts;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new BudgetUpdated(this));
        }

        return this;
    }

    public Budget AddBudgetLine(DefaultIdType accountId, decimal budgetedAmount, string? description = null)
    {
        if (Status == "Approved" || Status == "Active")
            throw new BudgetCannotBeModifiedException(Id);

        if (budgetedAmount < 0)
            throw new InvalidBudgetAmountException();

        var existingLine = _budgetLines.FirstOrDefault(bl => bl.AccountId == accountId);
        if (existingLine != null)
            throw new BudgetLineAlreadyExistsException(Id, accountId);

        var budgetLine = BudgetLine.Create(Id, accountId, budgetedAmount, description);
        _budgetLines.Add(budgetLine);

        RecalculateTotalBudgetedAmount();
        QueueDomainEvent(new BudgetLineAdded(Id, accountId, budgetedAmount));
        return this;
    }

    public Budget UpdateBudgetLine(DefaultIdType accountId, decimal? budgetedAmount, string? description)
    {
        if (Status == "Approved" || Status == "Active")
            throw new BudgetCannotBeModifiedException(Id);

        var budgetLine = _budgetLines.FirstOrDefault(bl => bl.AccountId == accountId);
        if (budgetLine == null)
            throw new BudgetLineNotFoundException(Id, accountId);

        budgetLine.Update(budgetedAmount, description);
        RecalculateTotalBudgetedAmount();
        QueueDomainEvent(new BudgetLineUpdated(Id, accountId, budgetLine.BudgetedAmount));
        return this;
    }

    public Budget RemoveBudgetLine(DefaultIdType accountId)
    {
        if (Status == "Approved" || Status == "Active")
            throw new BudgetCannotBeModifiedException(Id);

        var budgetLine = _budgetLines.FirstOrDefault(bl => bl.AccountId == accountId);
        if (budgetLine == null)
            throw new BudgetLineNotFoundException(Id, accountId);

        _budgetLines.Remove(budgetLine);
        RecalculateTotalBudgetedAmount();
        QueueDomainEvent(new BudgetLineRemoved(Id, accountId));
        return this;
    }

    public Budget Approve(string approvedBy)
    {
        if (Status == "Approved")
            throw new BudgetAlreadyApprovedException(Id);

        if (_budgetLines.Count == 0)
            throw new EmptyBudgetCannotBeApprovedException(Id);

        Status = "Approved";
        ApprovedDate = DateTime.UtcNow;
        ApprovedBy = approvedBy.Trim();

        QueueDomainEvent(new BudgetApproved(Id, ApprovedDate.Value, ApprovedBy));
        return this;
    }

    public Budget Activate()
    {
        if (Status != "Approved")
            throw new BudgetNotApprovedException(Id);

        Status = "Active";
        QueueDomainEvent(new BudgetActivated(Id, Name));
        return this;
    }

    public Budget Close()
    {
        if (Status != "Active")
            throw new BudgetNotApprovedException(Id);

        Status = "Closed";
        QueueDomainEvent(new BudgetClosed(Id, Name));
        return this;
    }

    public Budget UpdateActuals(DefaultIdType accountId, decimal actualAmount)
    {
        var budgetLine = _budgetLines.FirstOrDefault(bl => bl.AccountId == accountId);
        if (budgetLine == null)
            throw new InvalidOperationException("Budget line not found");

        budgetLine.UpdateActual(actualAmount);
        RecalculateTotalActualAmount();
        return this;
    }

    private void RecalculateTotalBudgetedAmount()
    {
        TotalBudgetedAmount = _budgetLines.Sum(bl => bl.BudgetedAmount);
    }

    private void RecalculateTotalActualAmount()
    {
        TotalActualAmount = _budgetLines.Sum(bl => bl.ActualAmount);
    }

    public decimal GetTotalVariance()
    {
        return TotalBudgetedAmount - TotalActualAmount;
    }

    public decimal GetVariancePercentage()
    {
        return TotalBudgetedAmount > 0 ? ((TotalBudgetedAmount - TotalActualAmount) / TotalBudgetedAmount) * 100 : 0;
    }
}

public class BudgetLine : BaseEntity
{
    private const int MaxBudgetLineDescriptionLength = 500;

    public DefaultIdType BudgetId { get; private set; }
    public DefaultIdType AccountId { get; private set; }
    public decimal BudgetedAmount { get; private set; }
    public decimal ActualAmount { get; private set; }
    public string? Description { get; private set; }

    private BudgetLine(DefaultIdType budgetId, DefaultIdType accountId, 
        decimal budgetedAmount, string? description = null)
    {
        if (budgetedAmount < 0)
            throw new InvalidBudgetAmountException();

        BudgetId = budgetId;
        AccountId = accountId;
        BudgetedAmount = budgetedAmount;
        ActualAmount = 0;

        var desc = description?.Trim();
        if (!string.IsNullOrEmpty(desc) && desc.Length > MaxBudgetLineDescriptionLength)
            desc = desc.Substring(0, MaxBudgetLineDescriptionLength);
        Description = desc;
    }

    public static BudgetLine Create(DefaultIdType budgetId, DefaultIdType accountId,
        decimal budgetedAmount, string? description = null)
    {
        if (budgetedAmount < 0)
            throw new InvalidBudgetAmountException();

        return new BudgetLine(budgetId, accountId, budgetedAmount, description);
    }

    public BudgetLine Update(decimal? budgetedAmount, string? description)
    {
        if (budgetedAmount.HasValue && BudgetedAmount != budgetedAmount.Value)
        {
            if (budgetedAmount.Value < 0)
                throw new InvalidBudgetAmountException();
            BudgetedAmount = budgetedAmount.Value;
        }

        if (description != Description)
        {
            var desc = description?.Trim();
            if (!string.IsNullOrEmpty(desc) && desc.Length > MaxBudgetLineDescriptionLength)
                desc = desc.Substring(0, MaxBudgetLineDescriptionLength);
            Description = desc;
        }

        return this;
    }

    public BudgetLine UpdateActual(decimal actualAmount)
    {
        ActualAmount = actualAmount;
        return this;
    }

    public decimal GetVariance()
    {
        return BudgetedAmount - ActualAmount;
    }

    public decimal GetVariancePercentage()
    {
        return BudgetedAmount > 0 ? ((BudgetedAmount - ActualAmount) / BudgetedAmount) * 100 : 0;
    }
}
