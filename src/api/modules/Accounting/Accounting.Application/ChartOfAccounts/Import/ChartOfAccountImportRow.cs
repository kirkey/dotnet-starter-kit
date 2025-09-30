namespace Accounting.Application.ChartOfAccounts.Import;

/// <summary>
/// Strongly-typed row parsed from the import Excel file for Chart of Accounts.
/// Represents a single account entry with all required and optional fields for account creation.
/// </summary>
public sealed class ChartOfAccountImportRow
{
    /// <summary>
    /// The unique account code (e.g., "101" for Cash, "403" for Overhead Line Expenses). Required.
    /// </summary>
    public string? AccountCode { get; init; }

    /// <summary>
    /// The official account name (e.g., "Cash and Cash Equivalents"). Required.
    /// </summary>
    public string? AccountName { get; init; }

    /// <summary>
    /// The account type: Asset, Liability, Equity, Revenue, or Expense. Required.
    /// </summary>
    public string? AccountType { get; init; }

    /// <summary>
    /// The USOA category (e.g., Production, Transmission, Distribution). Required.
    /// </summary>
    public string? UsoaCategory { get; init; }

    /// <summary>
    /// Optional parent account ID for hierarchical structures.
    /// </summary>
    public DefaultIdType? ParentAccountId { get; init; }

    /// <summary>
    /// Optional dotted parent code path for hierarchy representation (e.g., "100.101").
    /// </summary>
    public string? ParentCode { get; init; }

    /// <summary>
    /// Initial balance of the account. Default: 0.00.
    /// </summary>
    public decimal? Balance { get; init; }

    /// <summary>
    /// Whether the account is a control account that disallows direct postings.
    /// </summary>
    public bool? IsControlAccount { get; init; }

    /// <summary>
    /// Normal balance side: "Debit" or "Credit".
    /// </summary>
    public string? NormalBalance { get; init; }

    /// <summary>
    /// Whether this account conforms to USOA standards.
    /// </summary>
    public bool? IsUsoaCompliant { get; init; }

    /// <summary>
    /// Optional regulatory classification (e.g., FERC category text).
    /// </summary>
    public string? RegulatoryClassification { get; init; }

    /// <summary>
    /// Whether the account is active and can be used in postings.
    /// </summary>
    public bool? IsActive { get; init; }

    /// <summary>
    /// Detailed description of the account purpose and usage.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Additional notes or comments about the account.
    /// </summary>
    public string? Notes { get; init; }
}
