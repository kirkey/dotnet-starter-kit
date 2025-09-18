namespace Store.Domain.Events;

public record CashDrawerSessionOpened : DomainEvent { public CashDrawerSession Session { get; init; } = default!; }
public record CashDrawerTransactionAdded : DomainEvent { public CashDrawerSession Session { get; init; } = default!; public CashDrawerTransaction Transaction { get; init; } = default!; }
public record CashDrawerSessionClosed : DomainEvent { public CashDrawerSession Session { get; init; } = default!; }

