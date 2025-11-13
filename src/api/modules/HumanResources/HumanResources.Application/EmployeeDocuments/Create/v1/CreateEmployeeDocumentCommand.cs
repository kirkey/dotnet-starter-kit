namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Create.v1;

/// <summary>
/// Command to create an employee document.
/// </summary>
public sealed record CreateEmployeeDocumentCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType EmployeeId,
    [property: DefaultValue("Contract")] string DocumentType,
    [property: DefaultValue("Employment Contract")] string Title,
    [property: DefaultValue(null)] string? FileName = null,
    [property: DefaultValue(null)] string? FilePath = null,
    [property: DefaultValue(null)] long? FileSize = null,
    [property: DefaultValue(null)] DateTime? ExpiryDate = null,
    [property: DefaultValue(null)] string? IssueNumber = null,
    [property: DefaultValue(null)] DateTime? IssueDate = null,
    [property: DefaultValue(null)] string? Notes = null) : IRequest<CreateEmployeeDocumentResponse>;

