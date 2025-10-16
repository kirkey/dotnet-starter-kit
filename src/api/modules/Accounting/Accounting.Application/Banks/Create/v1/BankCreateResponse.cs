namespace Accounting.Application.Banks.Create.v1;

/// <summary>
/// Response returned after successfully creating a bank.
/// </summary>
/// <param name="Id">The unique identifier of the newly created bank.</param>
public sealed record BankCreateResponse(DefaultIdType Id);


