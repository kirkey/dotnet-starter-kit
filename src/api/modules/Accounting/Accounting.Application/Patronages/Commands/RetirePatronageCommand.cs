namespace Accounting.Application.Patronages.Commands;

public sealed class RetirePatronageCommand : IRequest<DefaultIdType>
{
    public DefaultIdType PatronageCapitalId { get; init; }
    public decimal Amount { get; init; }
    public DateTime RetirementDate { get; init; } = DateTime.UtcNow;
    public DefaultIdType DebitAccountId { get; init; }
    public DefaultIdType CreditAccountId { get; init; }
    public string? Description { get; init; }
}

