namespace FSH.Starter.WebApi.HumanResources.Domain.Exceptions;

/// <summary>
/// Exception thrown when document template is not found.
/// </summary>
public class DocumentTemplateNotFoundException(DefaultIdType id)
    : NotFoundException($"Document template with ID '{id}' was not found.");

/// <summary>
/// Exception thrown when generated document is not found.
/// </summary>
public class GeneratedDocumentNotFoundException(DefaultIdType id)
    : NotFoundException($"Generated document with ID '{id}' was not found.");

/// <summary>
/// Exception thrown when document operation is invalid for current status.
/// </summary>
public class InvalidDocumentStatusException(string currentStatus, string operation)
    : BadRequestException($"Cannot {operation} a {currentStatus} document.");

/// <summary>
/// Exception thrown when template variables are missing.
/// </summary>
public class MissingTemplateVariablesException(string missingVariables)
    : BadRequestException($"Missing required template variables: {missingVariables}");

