namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Update.v1;

public sealed record UpdateEmployeeDocumentCommand(
    DefaultIdType Id,
    [property: DefaultValue(null)] string? Title = null,
    [property: DefaultValue(null)] DateTime? ExpiryDate = null,
    [property: DefaultValue(null)] string? IssueNumber = null,
    [property: DefaultValue(null)] DateTime? IssueDate = null,
    [property: DefaultValue(null)] string? Notes = null,
    [property: DefaultValue(null)] string? FileName = null,
    [property: DefaultValue(null)] string? FilePath = null,
    [property: DefaultValue(null)] long? FileSize = null) : IRequest<UpdateEmployeeDocumentResponse>;

