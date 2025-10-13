using Accounting.Application.ChartOfAccounts.Specs;
using Accounting.Domain.Entities;
using FSH.Framework.Core.Storage.Commands;

namespace Accounting.Application.ChartOfAccounts.Import.v1;

/// <summary>
/// Imports chart of accounts from an Excel file and returns detailed import results.
/// Handles validation, duplicate checking, and account hierarchy relationships.
/// </summary>
public sealed class ImportChartOfAccountsHandler(
    ILogger<ImportChartOfAccountsHandler> logger,
    IChartOfAccountImportParser parser,
    [FromKeyedServices("accounting:chartofaccounts")] IRepository<ChartOfAccount> repository,
    [FromKeyedServices("accounting:chartofaccounts")] IReadRepository<ChartOfAccount> readRepository)
    : IRequestHandler<ImportChartOfAccountsCommand, ImportResponse>
{
    /// <summary>
    /// Handles the import command and returns detailed import results.
    /// </summary>
    /// <param name="request">The import command containing the file to process.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>ImportResponse containing the number of accounts successfully imported, failed count, and errors.</returns>
    public async Task<ImportResponse> Handle(ImportChartOfAccountsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.File);

        var rows = await parser.ParseAsync(request.File, cancellationToken).ConfigureAwait(false);
        if (rows.Count == 0)
        {
            logger.LogInformation("Chart of accounts import: file had no rows");
            return ImportResponse.Success(0);
        }

        int imported = 0;
        var errors = new List<string>();

        for (int i = 0; i < rows.Count; i++)
        {
            var rowIndex = i + 1; // human-friendly
            var row = rows[i];

            try
            {
                // Strict validations: AccountCode, AccountName, AccountType, and UsoaCategory are required
                if (string.IsNullOrWhiteSpace(row.AccountCode))
                {
                    errors.Add($"Row {rowIndex}: Account Code is required");
                    continue;
                }
                if (string.IsNullOrWhiteSpace(row.AccountName))
                {
                    errors.Add($"Row {rowIndex}: Account Name is required");
                    continue;
                }
                if (string.IsNullOrWhiteSpace(row.AccountType))
                {
                    errors.Add($"Row {rowIndex}: Account Type is required");
                    continue;
                }
                if (string.IsNullOrWhiteSpace(row.UsoaCategory))
                {
                    errors.Add($"Row {rowIndex}: USOA Category is required");
                    continue;
                }

                // Normalize and validate inputs
                var accountCode = row.AccountCode.Trim();
                if (accountCode.Length > 16)
                {
                    errors.Add($"Row {rowIndex}: Account Code cannot exceed 16 characters");
                    continue;
                }

                var accountName = row.AccountName.Trim();
                if (accountName.Length > 1024) accountName = accountName[..1024];

                var accountType = row.AccountType.Trim();
                if (!IsValidAccountType(accountType))
                {
                    errors.Add($"Row {rowIndex}: Invalid Account Type '{accountType}'. Must be Asset, Liability, Equity, Revenue, or Expense");
                    continue;
                }

                var usoaCategory = row.UsoaCategory.Trim();
                if (!IsValidUsoaCategory(usoaCategory))
                {
                    errors.Add($"Row {rowIndex}: Invalid USOA Category '{usoaCategory}'");
                    continue;
                }

                // Check for duplicate account code
                var existingAccount = await readRepository.FirstOrDefaultAsync(
                    new ChartOfAccountByCodeSpec(accountCode), cancellationToken).ConfigureAwait(false);
                if (existingAccount is not null)
                {
                    errors.Add($"Row {rowIndex}: Account Code '{accountCode}' already exists");
                    continue;
                }

                // Process optional fields with defaults
                string? parentCode = string.IsNullOrWhiteSpace(row.ParentCode) ? null : row.ParentCode.Trim();
                if (parentCode?.Length > 16) parentCode = parentCode[..16];

                var balance = row.Balance ?? 0m;
                var isControlAccount = row.IsControlAccount ?? false;
                var normalBalance = string.IsNullOrWhiteSpace(row.NormalBalance) ? "Debit" : row.NormalBalance.Trim();
                var isUsoaCompliant = row.IsUsoaCompliant ?? true;
                var isActive = row.IsActive ?? true;

                string? regulatoryClassification = string.IsNullOrWhiteSpace(row.RegulatoryClassification) 
                    ? null : row.RegulatoryClassification.Trim();
                if (regulatoryClassification?.Length > 256) regulatoryClassification = regulatoryClassification[..256];

                string? description = string.IsNullOrWhiteSpace(row.Description) ? null : row.Description.Trim();
                if (description?.Length > 2048) description = description[..2048];

                string? notes = string.IsNullOrWhiteSpace(row.Notes) ? null : row.Notes.Trim();
                if (notes?.Length > 2048) notes = notes[..2048];

                // Create the account using domain factory method
                var account = ChartOfAccount.Create(
                    accountCode,
                    accountName,
                    accountType,
                    usoaCategory,
                    row.ParentAccountId,
                    parentCode,
                    balance,
                    isControlAccount,
                    normalBalance,
                    isUsoaCompliant,
                    regulatoryClassification,
                    description,
                    notes);

                if (!isActive)
                {
                    account.Deactivate();
                }

                await repository.AddAsync(account, cancellationToken).ConfigureAwait(false);
                imported++;

                logger.LogDebug("Imported account: {AccountCode} - {AccountName}", accountCode, accountName);
            }
            catch (Exception ex)
            {
                errors.Add($"Row {rowIndex}: {ex.Message}");
                logger.LogWarning(ex, "Failed to import account at row {RowIndex}", rowIndex);
            }
        }

        var failedCount = rows.Count - imported;

        if (errors.Count > 0)
        {
            logger.LogWarning("Chart of accounts import completed with {ErrorCount} errors: {Errors}", 
                errors.Count, string.Join("; ", errors.Take(10)));
        }

        logger.LogInformation("Successfully imported {ImportedCount} of {TotalRows} chart of accounts (Failed: {FailedCount})", 
            imported, rows.Count, failedCount);

        return failedCount > 0
            ? ImportResponse.PartialSuccess(imported, failedCount, errors)
            : ImportResponse.Success(imported);
    }

    /// <summary>
    /// Validates if the account type is one of the allowed values.
    /// </summary>
    private static bool IsValidAccountType(string accountType)
    {
        var validTypes = new[] { "Asset", "Liability", "Equity", "Revenue", "Expense" };
        return validTypes.Contains(accountType, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Validates if the USOA category is valid.
    /// </summary>
    private static bool IsValidUsoaCategory(string usoaCategory)
    {
        var validCategories = new[] { "Production", "Transmission", "Distribution", "Customer", "Administrative", "General" };
        return validCategories.Contains(usoaCategory, StringComparer.OrdinalIgnoreCase);
    }
}

