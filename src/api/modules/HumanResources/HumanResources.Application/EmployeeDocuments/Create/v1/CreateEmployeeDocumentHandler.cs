namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Create.v1;

public sealed class CreateEmployeeDocumentHandler(
    ILogger<CreateEmployeeDocumentHandler> logger,
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> employeeRepository,
    [FromKeyedServices("hr:documents")] IRepository<EmployeeDocument> repository)
    : IRequestHandler<CreateEmployeeDocumentCommand, CreateEmployeeDocumentResponse>
{
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
            "Employee document created with ID {DocumentId}, Employee {EmployeeId}, Type {DocumentType}",
            document.Id,
            document.EmployeeId,
            document.DocumentType);

        return new CreateEmployeeDocumentResponse(document.Id);
    }
}

