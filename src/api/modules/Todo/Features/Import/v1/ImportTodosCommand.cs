using FSH.Framework.Core.Storage.Commands;
using FSH.Framework.Core.Storage.File.Features;
using MediatR;

namespace FSH.Starter.WebApi.Todo.Features.Import.v1;

/// <summary>
/// Command for importing Todos from an Excel file.
/// Uses the generic import infrastructure for consistent processing.
/// </summary>
public sealed record ImportTodosCommand : IRequest<ImportResponse>
{
    /// <summary>
    /// The uploaded Excel file containing todos to import.
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
