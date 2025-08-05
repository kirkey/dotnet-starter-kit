using Accounting.Domain.Events.Budget;
using Accounting.Domain.Exceptions;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace Accounting.Domain;

public class Budget : AuditableEntity, IAggregateRoot
{
    public DefaultIdType PeriodId { get; private set; }
    public int FiscalYear { get; private set; }
    public string BudgetType { get; private set; } // Operating, Capital, Cash Flow
    public string Status { get; private set; } // Draft, Approved, Active, Closed
    public decimal TotalBudgetedAmount { get; private set; }
    public decimal TotalActualAmount { get; private set; }
    public DateTime? ApprovedDate { get; private set; }
    public string? ApprovedBy { get; private set; }

    private readonly List<BudgetLine> _budgetLines = new();
    public IReadOnlyCollection<BudgetLine> BudgetLines => _budgetLines.AsReadOnly();

    private Budget(string budgetName, DefaultIdType periodId, int fiscalYear, string budgetType, string? description = null, string? notes = null)
    {
        Name = budgetName.Trim();
        PeriodId = periodId;
        FiscalYear = fiscalYear;
        BudgetType = budgetType.Trim();
        Status = "Draft";
        TotalBudgetedAmount = 0;
        TotalActualAmount = 0;
        Description = description?.Trim();
        Notes = notes?.Trim();

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
            Name = budgetName.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(budgetType) && BudgetType != budgetType)
        {
            BudgetType = budgetType.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(status) && Status != status)
        {
            Status = status.Trim();
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
    public DefaultIdType BudgetId { get; private set; }
    public DefaultIdType AccountId { get; private set; }
    public decimal BudgetedAmount { get; private set; }
    public decimal ActualAmount { get; private set; }
    public string? Description { get; private set; }

    private BudgetLine(DefaultIdType budgetId, DefaultIdType accountId, 
        decimal budgetedAmount, string? description = null)
    {
        BudgetId = budgetId;
        AccountId = accountId;
        BudgetedAmount = budgetedAmount;
        ActualAmount = 0;
        Description = description?.Trim();
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
            Description = description?.Trim();
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
