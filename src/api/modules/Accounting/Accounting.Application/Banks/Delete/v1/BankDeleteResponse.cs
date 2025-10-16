namespace Accounting.Application.Banks.Delete.v1;

/// <summary>
/// Response returned after successfully deleting a bank.
/// </summary>
/// <param name="Id">The unique identifier of the deleted bank.</param>
public sealed record BankDeleteResponse(DefaultIdType Id);

