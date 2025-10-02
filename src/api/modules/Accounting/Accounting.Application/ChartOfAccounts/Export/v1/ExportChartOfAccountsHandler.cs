using FSH.Framework.Core.Storage;
using Accounting.Application.ChartOfAccounts.Specs;
using Accounting.Domain.Entities;

namespace Accounting.Application.ChartOfAccounts.Export.v1;

/// <summary>
/// Handler for exporting chart of accounts to Excel format using the framework's DataExport service.
/// Supports filtering by account type, USOA category, search terms, and various criteria.
/// </summary>
public sealed class ExportChartOfAccountsHandler(
    ILogger<ExportChartOfAccountsHandler> logger,
    IDataExport dataExport,
    [FromKeyedServices("accounting:chartofaccounts")] IReadRepository<ChartOfAccount> repository)
    : IRequestHandler<ExportChartOfAccountsQuery, ExportChartOfAccountsResponse>
{
    public async Task<ExportChartOfAccountsResponse> Handle(ExportChartOfAccountsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Starting chart of accounts export with filters: AccountType={AccountType}, UsoaCategory={UsoaCategory}, SearchTerm={SearchTerm}, OnlyControlAccounts={OnlyControlAccounts}", 
            request.AccountType, request.UsoaCategory, request.SearchTerm, request.OnlyControlAccounts);

        // Use the single specification that handles all filtering
        var spec = new ExportChartOfAccountsSpec(request);

        // Fetch data from repository
        var accounts = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("Found {Count} chart of accounts matching export criteria", accounts.Count);

        // Transform domain entities to export DTOs
        var exportData = accounts.Select(MapToExportDto).ToList();

        // Handle empty data case - create empty list with proper structure
        if (exportData.Count == 0)
        {
            exportData.Add(new ChartOfAccountExportDto()); // Add empty row to maintain Excel structure
        }

        // Use framework's DataExport service directly to generate Excel file
        var excelBytes = dataExport.ListToByteArray(exportData);
        
        logger.LogInformation("Successfully exported {Count} chart of accounts to Excel", accounts.Count);

        return ExportChartOfAccountsResponse.Create(excelBytes, accounts.Count);
    }

    /// <summary>
    /// Maps a ChartOfAccount domain entity to export DTO with proper formatting.
    /// </summary>
    /// <param name="account">Domain entity to map</param>
    /// <returns>Export DTO ready for Excel generation</returns>
    private static ChartOfAccountExportDto MapToExportDto(ChartOfAccount account)
    {
        return new ChartOfAccountExportDto
        {
            AccountCode = account.AccountCode,
            AccountName = account.AccountName,
            AccountType = account.AccountType,
            UsoaCategory = account.UsoaCategory,
            ParentCode = account.ParentCode,
            Balance = account.Balance,
            IsControlAccount = account.IsControlAccount,
            NormalBalance = account.NormalBalance,
            AccountLevel = account.AccountLevel,
            AllowDirectPosting = account.AllowDirectPosting,
            IsUsoaCompliant = account.IsUsoaCompliant,
            RegulatoryClassification = account.RegulatoryClassification,
            IsActive = account.IsActive,
            Description = account.Description,
            Notes = account.Notes,
        };
    }
}
