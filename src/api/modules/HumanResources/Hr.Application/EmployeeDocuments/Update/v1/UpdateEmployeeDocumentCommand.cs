namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Update.v1;

/// <summary>
/// Command to update an employee document.
/// </summary>
public sealed record UpdateEmployeeDocumentCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue("Contract")] string? DocumentType = null,
    [property: DefaultValue("Employment Contract")] string? Title = null,
    [property: DefaultValue(null)] DateTime? ExpiryDate = null,
    [property: DefaultValue("CONTRACT-2024-001")] string? IssueNumber = null,
    [property: DefaultValue(null)] DateTime? IssueDate = null,
    [property: DefaultValue(null)] string? Notes = null) : IRequest<UpdateEmployeeDocumentResponse>;
