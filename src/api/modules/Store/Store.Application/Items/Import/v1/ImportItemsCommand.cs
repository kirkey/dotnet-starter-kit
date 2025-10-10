using FSH.Framework.Core.Storage.Commands;
using FSH.Framework.Core.Storage.File.Features;

namespace FSH.Starter.WebApi.Store.Application.Items.Import.v1;

/// <summary>
/// Command for importing Items from an Excel file.
/// Uses the generic import infrastructure for consistent processing.
/// </summary>
public sealed record ImportItemsCommand : IRequest<ImportResponse>
{
    /// <summary>
    /// The uploaded Excel file containing items to import.
    /// </summary>
    public required FileUploadCommand File { get; init; }

    /// <summary>
    /// The worksheet name to import from. Defaults to "Sheet1".
    /// </summary>
    public string SheetName { get; init; } = "Sheet1";

    /// <summary>
    /// Indicates whether to validate file structure before processing.
    /// </summary>
    public bool ValidateStructure { get; init; } = true;
}

