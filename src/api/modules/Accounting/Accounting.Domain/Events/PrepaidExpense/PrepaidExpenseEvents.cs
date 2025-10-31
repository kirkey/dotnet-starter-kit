namespace Accounting.Domain.Events.PrepaidExpense;

public record PrepaidExpenseCreated(DefaultIdType Id, string PrepaidNumber, string Description, decimal TotalAmount, DateTime StartDate, DateTime EndDate, string? Notes) : DomainEvent;

public record PrepaidExpenseUpdated(DefaultIdType Id, string PrepaidNumber, string Description, string? Notes) : DomainEvent;

public record PrepaidExpenseDeleted(DefaultIdType Id) : DomainEvent;

public record PrepaidExpenseAmortized(DefaultIdType Id, string PrepaidNumber, decimal AmortizationAmount, decimal RemainingBalance, DateTime PostingDate) : DomainEvent;

public record PrepaidExpenseFullyAmortized(DefaultIdType Id, string PrepaidNumber, string Description, decimal TotalAmount, DateTime CompletionDate) : DomainEvent;

public record PrepaidExpenseClosed(DefaultIdType Id, string PrepaidNumber, string Description) : DomainEvent;

public record PrepaidExpenseCancelled(DefaultIdType Id, string PrepaidNumber, string Reason) : DomainEvent;

