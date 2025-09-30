namespace Accounting.Application.Payees.Export.v1;

/// <summary>
/// Query to export payees to an Excel file.
/// Supports optional filtering by expense account, search criteria, and payee status.
/// </summary>
/// <param name="ExpenseAccountCode">Optional expense account code to filter payees</param>
/// <param name="SearchTerm">Optional search term to filter by payee code, name, or TIN</param>
/// <param name="IncludeInactive">Whether to include inactive payees in export (default: false)</param>
/// <param name="HasTin">Filter payees that have TIN specified (default: null for all)</param>
/// <param name="HasExpenseAccount">Filter payees that have expense account mapping (default: null for all)</param>
public sealed record ExportPayeesQuery(
    string? ExpenseAccountCode = null,
    string? SearchTerm = null,
    bool IncludeInactive = false,
    bool? HasTin = null,
    bool? HasExpenseAccount = null) : IRequest<ExportPayeesResponse>;
