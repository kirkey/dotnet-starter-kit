namespace Accounting.Application.Banks.Update.v1;

/// <summary>
/// Response returned after successfully updating a bank.
/// </summary>
/// <param name="Id">The unique identifier of the updated bank.</param>
public sealed record BankUpdateResponse(DefaultIdType Id);

