namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Update.v1;

/// <summary>
/// Response for updating an employee document.
/// </summary>
/// <param name="Id">The identifier of the updated document.</param>
public sealed record UpdateEmployeeDocumentResponse(DefaultIdType Id);

