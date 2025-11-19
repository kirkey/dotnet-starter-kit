namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Delete.v1;

/// <summary>
/// Command to delete an employee document.
/// </summary>
public sealed record DeleteEmployeeDocumentCommand(DefaultIdType Id) : IRequest<DeleteEmployeeDocumentResponse>;

