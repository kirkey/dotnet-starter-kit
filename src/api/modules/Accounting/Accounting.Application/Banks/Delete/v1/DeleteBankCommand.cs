namespace Accounting.Application.Banks.Delete.v1;

/// <summary>
/// Command for deleting a bank entity from the accounting system.
/// </summary>
/// <param name="Id">The unique identifier of the bank to delete.</param>
public sealed record DeleteBankCommand(DefaultIdType Id) : IRequest<BankDeleteResponse>;
