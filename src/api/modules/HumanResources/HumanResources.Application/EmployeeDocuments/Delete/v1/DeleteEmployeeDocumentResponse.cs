namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Delete.v1;

/// <summary>
/// Response for deleting an employee document.
/// </summary>
/// <param name="Id">The identifier of the deleted document.</param>
public sealed record DeleteEmployeeDocumentResponse(DefaultIdType Id);

