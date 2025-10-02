using Accounting.Application.Payees.Specs;
using Accounting.Domain.Entities;

namespace Accounting.Application.Payees.Import.v1;

/// <summary>
/// Imports payees from an Excel file and returns the total number of successfully imported payees.
/// Handles validation, duplicate checking, and vendor master data management.
/// </summary>
public sealed class ImportPayeesHandler(
    ILogger<ImportPayeesHandler> logger,
    IPayeeImportParser parser,
    [FromKeyedServices("accounting:payees")] IRepository<Payee> repository,
    [FromKeyedServices("accounting:payees")] IReadRepository<Payee> readRepository)
    : IRequestHandler<ImportPayeesCommand, int>
{
    /// <summary>
    /// Handles the import command and returns the count of imported payees.
    /// </summary>
    /// <param name="request">The import command containing the file to process.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The number of payees successfully imported.</returns>
    public async Task<int> Handle(ImportPayeesCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.File);

        var rows = await parser.ParseAsync(request.File, cancellationToken).ConfigureAwait(false);
        if (rows.Count == 0)
        {
            var noRowsMsg = "payees import: file had no rows";
            logger.LogInformation(noRowsMsg);
            return 0;
        }

        int imported = 0;
        var errors = new List<string>();

        for (int i = 0; i < rows.Count; i++)
        {
            var rowIndex = i + 1; // human-friendly
            var row = rows[i];

            try
            {
                // Strict validations: PayeeCode and Name are required
                if (string.IsNullOrWhiteSpace(row.PayeeCode))
                {
                    errors.Add($"Row {rowIndex}: Payee Code is required");
                    continue;
                }
                if (string.IsNullOrWhiteSpace(row.Name))
                {
                    errors.Add($"Row {rowIndex}: Name is required");
                    continue;
                }

                // Normalize and validate inputs
                var payeeCode = row.PayeeCode.Trim();
                if (payeeCode.Length > 50)
                {
                    errors.Add($"Row {rowIndex}: Payee Code cannot exceed 50 characters");
                    continue;
                }

                var name = row.Name.Trim();
                if (name.Length > 200) name = name[..200];

                // Check for duplicate payee code
                var existingPayee = await readRepository.FirstOrDefaultAsync(
                    new PayeeByCodeSpec(payeeCode), cancellationToken).ConfigureAwait(false);
                if (existingPayee is not null)
                {
                    errors.Add($"Row {rowIndex}: Payee Code '{payeeCode}' already exists");
                    continue;
                }

                // Check for duplicate TIN if provided
                if (!string.IsNullOrWhiteSpace(row.Tin))
                {
                    var tinToCheck = row.Tin.Trim();
                    var existingTin = await readRepository.FirstOrDefaultAsync(
                        new PayeeByTinSpec(tinToCheck), cancellationToken).ConfigureAwait(false);
                    if (existingTin is not null)
                    {
                        errors.Add($"Row {rowIndex}: TIN '{tinToCheck}' already exists for another payee");
                        continue;
                    }
                }

                // Process optional fields with validation
                string? address = string.IsNullOrWhiteSpace(row.Address) ? null : row.Address.Trim();
                if (address?.Length > 500) address = address[..500];

                string? expenseAccountCode = string.IsNullOrWhiteSpace(row.ExpenseAccountCode) 
                    ? null : row.ExpenseAccountCode.Trim();
                if (expenseAccountCode?.Length > 50) expenseAccountCode = expenseAccountCode[..50];

                string? expenseAccountName = string.IsNullOrWhiteSpace(row.ExpenseAccountName) 
                    ? null : row.ExpenseAccountName.Trim();
                if (expenseAccountName?.Length > 200) expenseAccountName = expenseAccountName[..200];

                string? tin = string.IsNullOrWhiteSpace(row.Tin) ? null : row.Tin.Trim();
                if (tin?.Length > 50) tin = tin[..50];

                string? description = string.IsNullOrWhiteSpace(row.Description) ? null : row.Description.Trim();
                if (description?.Length > 1000) description = description[..1000];

                string? notes = string.IsNullOrWhiteSpace(row.Notes) ? null : row.Notes.Trim();
                if (notes?.Length > 2000) notes = notes[..2000];

                // Create the payee using domain factory method
                var payee = Payee.Create(
                    payeeCode,
                    name,
                    address,
                    expenseAccountCode,
                    expenseAccountName,
                    tin,
                    description,
                    notes);

                // Handle active status
                var isActive = row.IsActive ?? true;
                if (!isActive)
                {
                    payee.Deactivate();
                }

                await repository.AddAsync(payee, cancellationToken).ConfigureAwait(false);
                imported++;

                logger.LogDebug("Imported payee: {PayeeCode} - {Name}", payeeCode, name);
            }
            catch (Exception ex)
            {
                errors.Add($"Row {rowIndex}: {ex.Message}");
                logger.LogWarning(ex, "Failed to import payee at row {RowIndex}", rowIndex);
            }
        }

        if (errors.Count > 0)
        {
            logger.LogWarning("Payees import completed with {ErrorCount} errors: {Errors}", 
                errors.Count, string.Join("; ", errors.Take(10)));
        }

        logger.LogInformation("Successfully imported {ImportedCount} of {TotalRows} payees", 
            imported, rows.Count);

        return imported;
    }
}
