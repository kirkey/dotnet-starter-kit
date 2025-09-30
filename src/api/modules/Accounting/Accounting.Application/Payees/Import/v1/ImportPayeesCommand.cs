using FSH.Framework.Core.Storage.File.Features;

namespace Accounting.Application.Payees.Import.v1;

/// <summary>
/// Command to import payees from an Excel file (.xlsx). Returns the total imported count.
/// Validates and creates payee entries with proper vendor master data management.
/// </summary>
public sealed record ImportPayeesCommand(FileUploadCommand File) : IRequest<int>;
