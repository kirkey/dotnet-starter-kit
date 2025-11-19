namespace FSH.Starter.WebApi.HumanResources.Domain.Exceptions;

/// <summary>
/// Exception thrown when an employee document is not found.
/// </summary>
public class EmployeeDocumentNotFoundException(DefaultIdType id)
    : NotFoundException($"Employee document with ID '{id}' was not found.");

/// <summary>
/// Exception thrown when invalid document type is provided.
/// </summary>
public class InvalidDocumentTypeException(string documentType) : BadRequestException(
    $"Document type '{documentType}' is not valid. Valid types are: Contract, Certification, License, Identity, Medical, Other.");

/// <summary>
/// Exception thrown when document has expired.
/// </summary>
public class DocumentExpiredException(string documentTitle, DateTime expiryDate)
    : BadRequestException($"Document '{documentTitle}' expired on {expiryDate:MMM d, yyyy}.");

