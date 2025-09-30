namespace Accounting.Application.ChartOfAccounts.Export.v1;

/// <summary>
/// Query to export chart of accounts to an Excel file.
/// Supports optional filtering by account type, USOA category, or search criteria.
/// </summary>
/// <param name="AccountType">Optional account type to filter (Asset, Liability, Equity, Revenue, Expense)</param>
/// <param name="UsoaCategory">Optional USOA category to filter (Production, Transmission, Distribution, etc.)</param>
/// <param name="SearchTerm">Optional search term to filter by account code, name, or description</param>
/// <param name="IncludeInactive">Whether to include inactive accounts in export (default: false)</param>
/// <param name="OnlyControlAccounts">Whether to export only control accounts (default: false)</param>
/// <param name="OnlyDetailAccounts">Whether to export only detail accounts (default: false)</param>
/// <param name="ParentAccountId">Optional parent account ID to filter child accounts</param>
public sealed record ExportChartOfAccountsQuery(
    string? AccountType = null,
    string? UsoaCategory = null,
    string? SearchTerm = null,
    bool IncludeInactive = false,
    bool OnlyControlAccounts = false,
    bool OnlyDetailAccounts = false,
    DefaultIdType? ParentAccountId = null) : IRequest<ExportChartOfAccountsResponse>;
