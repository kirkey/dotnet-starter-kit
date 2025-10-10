using Accounting.Application.Payees.Specs;
using Accounting.Domain.Entities;
using FSH.Framework.Core.Storage;

namespace Accounting.Application.Payees.Export.v1;

/// <summary>
/// Handler for exporting payees to Excel format using the framework's DataExport service.
/// Supports filtering by expense account, search terms, and various criteria.
/// </summary>
public sealed class ExportPayeesHandler(
    ILogger<ExportPayeesHandler> logger,
    IDataExport dataExport,
    [FromKeyedServices("accounting:payees")] IReadRepository<Payee> repository)
    : IRequestHandler<ExportPayeesQuery, ExportPayeesResponse>
{
    public async Task<ExportPayeesResponse> Handle(ExportPayeesQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Starting payees export with filters: ExpenseAccountCode={ExpenseAccountCode}, SearchTerm={SearchTerm}, HasTin={HasTin}", 
            request.ExpenseAccountCode, request.SearchTerm, request.HasTin);

        // Use the single specification that handles all filtering
        var spec = new ExportPayeesSpec(request);

        // Fetch data from repository
        var payees = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("Found {Count} payees matching export criteria", payees.Count);

        // Transform domain entities to export DTOs
        var exportData = payees.Select(MapToExportDto).ToList();

        // Handle empty data case - create empty list with proper structure
        if (exportData.Count == 0)
        {
            exportData.Add(new PayeeExportDto()); // Add empty row to maintain Excel structure
        }

        // Use framework's DataExport service directly to generate Excel file
        var excelBytes = dataExport.ListToByteArray(exportData);
        
        logger.LogInformation("Successfully exported {Count} payees to Excel", payees.Count);

        return ExportPayeesResponse.Create(excelBytes, payees.Count);
    }

    /// <summary>
    /// Maps a Payee domain entity to export DTO with proper formatting.
    /// </summary>
    /// <param name="payee">Domain entity to map</param>
    /// <returns>Export DTO ready for Excel generation</returns>
    private static PayeeExportDto MapToExportDto(Payee payee)
    {
        return new PayeeExportDto
        {
            PayeeCode = payee.PayeeCode,
            Name = payee.Name,
            Address = payee.Address,
            ExpenseAccountCode = payee.ExpenseAccountCode,
            ExpenseAccountName = payee.ExpenseAccountName,
            Tin = payee.Tin,
            Description = payee.Description,
            Notes = payee.Notes,
            CreatedOn = payee.CreatedOn,
            LastModifiedOn = payee.LastModifiedOn
        };
    }
}
