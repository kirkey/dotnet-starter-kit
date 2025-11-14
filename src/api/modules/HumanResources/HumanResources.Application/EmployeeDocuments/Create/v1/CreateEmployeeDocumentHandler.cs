namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Create.v1;

/// <summary>
/// Handler for creating a new employee document.
/// </summary>
public sealed class CreateEmployeeDocumentHandler(
    ILogger<CreateEmployeeDocumentHandler> logger,
    [FromKeyedServices("hr:documents")] IRepository<EmployeeDocument> repository,
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> employeeRepository)
    : IRequestHandler<CreateEmployeeDocumentCommand, CreateEmployeeDocumentResponse>
{
    /// <summary>
    /// Handles the request to create an employee document.
    /// </summary>
    public async Task<CreateEmployeeDocumentResponse> Handle(
        CreateEmployeeDocumentCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var employee = await employeeRepository
            .GetByIdAsync(request.EmployeeId, cancellationToken)
            .ConfigureAwait(false);

        if (employee is null)
            throw new EmployeeNotFoundException(request.EmployeeId);

        var document = EmployeeDocument.Create(
            request.EmployeeId,
            request.DocumentType,
            request.Title,
            request.FileName,
            request.FilePath,
            request.FileSize,
            request.ExpiryDate,
            request.IssueNumber,
            request.IssueDate);

        if (!string.IsNullOrWhiteSpace(request.Notes))
            document.AddNotes(request.Notes);

        await repository.AddAsync(document, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Employee document created with ID {DocumentId}, Title {Title} for Employee {EmployeeId}",
            document.Id,
            document.Title,
            employee.Id);

        return new CreateEmployeeDocumentResponse(document.Id);
    }
}

