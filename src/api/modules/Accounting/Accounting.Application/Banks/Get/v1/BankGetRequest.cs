namespace Accounting.Application.Banks.Get.v1;

/// <summary>
/// Request for retrieving a bank by its unique identifier.
/// </summary>
/// <param name="Id">The unique identifier of the bank to retrieve.</param>
public sealed record BankGetRequest(DefaultIdType Id) : IRequest<BankResponse>;
