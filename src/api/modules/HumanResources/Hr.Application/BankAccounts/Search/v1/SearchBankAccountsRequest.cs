namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Search.v1;

/// <summary>
/// Request to search bank accounts.
/// </summary>
public sealed record SearchBankAccountsRequest(
    [property: DefaultValue(null)] DefaultIdType? EmployeeId = null,
    [property: DefaultValue(null)] string? BankName = null,
    [property: DefaultValue(null)] string? AccountType = null,
    [property: DefaultValue(null)] bool? IsActive = null,
    [property: DefaultValue(null)] bool? IsPrimary = null,
    [property: DefaultValue(1)] int PageNumber = 1,
    [property: DefaultValue(10)] int PageSize = 10
) : IRequest<PagedList<BankAccountDto>>;

/// <summary>
/// DTO for bank account search results (masked for security).
/// </summary>
public sealed record BankAccountDto(
    DefaultIdType Id,
    DefaultIdType EmployeeId,
    string BankName,
    string? Last4Digits,
    string AccountType,
    bool IsPrimary,
    bool IsActive,
    bool IsVerified);

