namespace FSH.Starter.WebApi.HumanResources.Domain.Exceptions;

/// <summary>
/// Exception thrown when document template is not found.
/// </summary>
public class DocumentTemplateNotFoundException : NotFoundException
{
    public DocumentTemplateNotFoundException(DefaultIdType id)
        : base($"Document template with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when generated document is not found.
/// </summary>
public class GeneratedDocumentNotFoundException : NotFoundException
{
    public GeneratedDocumentNotFoundException(DefaultIdType id)
        : base($"Generated document with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when document operation is invalid for current status.
/// </summary>
public class InvalidDocumentStatusException : BadRequestException
{
    public InvalidDocumentStatusException(string currentStatus, string operation)
        : base($"Cannot {operation} a {currentStatus} document.")
    {
    }
}

/// <summary>
/// Exception thrown when template variables are missing.
/// </summary>
public class MissingTemplateVariablesException : BadRequestException
{
    public MissingTemplateVariablesException(string missingVariables)
        : base($"Missing required template variables: {missingVariables}")
    {
    }
}

