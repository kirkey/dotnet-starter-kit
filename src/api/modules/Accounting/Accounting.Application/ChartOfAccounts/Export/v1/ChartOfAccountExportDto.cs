namespace Accounting.Application.ChartOfAccounts.Export.v1;

/// <summary>
/// Data Transfer Object for exporting Chart of Accounts to Excel format.
/// Contains all relevant account information formatted for spreadsheet export.
/// </summary>
public sealed class ChartOfAccountExportDto
{
    /// <summary>
    /// The unique account code (e.g., "101" for Cash, "403" for Overhead Line Expenses).
    /// </summary>
    public string AccountCode { get; set; } = string.Empty;

    /// <summary>
    /// The official account name (e.g., "Cash and Cash Equivalents").
    /// </summary>
    public string AccountName { get; set; } = string.Empty;

    /// <summary>
    /// The account type: Asset, Liability, Equity, Revenue, or Expense.
    /// </summary>
    public string AccountType { get; set; } = string.Empty;

    /// <summary>
    /// The USOA category (e.g., Production, Transmission, Distribution).
    /// </summary>
    public string UsoaCategory { get; set; } = string.Empty;

    /// <summary>
    /// Optional dotted parent code path for hierarchy representation.
    /// </summary>
    public string? ParentCode { get; set; }

    /// <summary>
    /// Current balance of the account.
    /// </summary>
    public decimal Balance { get; set; }

    /// <summary>
    /// Whether the account is a control account that disallows direct postings.
    /// </summary>
    public bool IsControlAccount { get; set; }

    /// <summary>
    /// Normal balance side: "Debit" or "Credit".
    /// </summary>
    public string NormalBalance { get; set; } = "Debit";

    /// <summary>
    /// Hierarchical depth computed from ParentCode.
    /// </summary>
    public int AccountLevel { get; set; }

    /// <summary>
    /// Whether direct postings are allowed.
    /// </summary>
    public bool AllowDirectPosting { get; set; }

    /// <summary>
    /// Whether this account conforms to USOA standards.
    /// </summary>
    public bool IsUsoaCompliant { get; set; }

    /// <summary>
    /// Optional regulatory classification (e.g., FERC category text).
    /// </summary>
    public string? RegulatoryClassification { get; set; }

    /// <summary>
    /// Whether the account is active and can be used in postings.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Detailed description of the account purpose and usage.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Additional notes or comments about the account.
    /// </summary>
    public string? Notes { get; set; }
}
