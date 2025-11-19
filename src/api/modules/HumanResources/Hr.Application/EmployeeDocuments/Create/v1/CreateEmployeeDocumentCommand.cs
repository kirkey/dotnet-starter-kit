namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Create.v1;

/// <summary>
/// Command to create a new employee document.
/// </summary>
public sealed record CreateEmployeeDocumentCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType EmployeeId,
    [property: DefaultValue("Contract")] string DocumentType,
    [property: DefaultValue("Employment Contract")] string Title,
    [property: DefaultValue("contract.pdf")] string? FileName = null,
    [property: DefaultValue("/docs/contracts/123")] string? FilePath = null,
    [property: DefaultValue(null)] long? FileSize = null,
    [property: DefaultValue(null)] DateTime? ExpiryDate = null,
    [property: DefaultValue("CONTRACT-2024-001")] string? IssueNumber = null,
    [property: DefaultValue(null)] DateTime? IssueDate = null,
    [property: DefaultValue(null)] string? Notes = null) : IRequest<CreateEmployeeDocumentResponse>;
