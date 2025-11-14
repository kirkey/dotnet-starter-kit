namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Create.v1;

/// <summary>
/// Response for creating an employee document.
/// </summary>
/// <param name="Id">The identifier of the created document.</param>
public sealed record CreateEmployeeDocumentResponse(DefaultIdType Id);

