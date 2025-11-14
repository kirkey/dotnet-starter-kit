using FSH.Starter.WebApi.HumanResources.Domain.Events;

namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents a document template for standardized documents.
/// Templates can be used to generate new documents quickly.
/// </summary>
public class DocumentTemplate : AuditableEntity, IAggregateRoot
{
    private DocumentTemplate() { }

    private DocumentTemplate(
        DefaultIdType id,
        string templateName,
        string documentType,
        string templateContent)
    {
        Id = id;
        TemplateName = templateName;
        DocumentType = documentType;
        TemplateContent = templateContent;
        IsActive = true;
        Version = 1;
    }

    /// <summary>
    /// Name of the template.
    /// </summary>
    public string TemplateName { get; private set; } = default!;

    /// <summary>
    /// Document type this template is for.
    /// </summary>
    public string DocumentType { get; private set; } = default!;

    /// <summary>
    /// Template content (HTML, plain text, or markup).
    /// </summary>
    public string TemplateContent { get; private set; } = default!;

    /// <summary>
    /// Template variables/placeholders (e.g., {{EmployeeName}}, {{StartDate}}).
    /// </summary>
    public string? TemplateVariables { get; private set; }

    /// <summary>
    /// Description of the template.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Template version for tracking changes.
    /// </summary>
    public int Version { get; private set; }

    /// <summary>
    /// Whether this template is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Creates a new document template.
    /// </summary>
    public static DocumentTemplate Create(
        string templateName,
        string documentType,
        string templateContent)
    {
        if (string.IsNullOrWhiteSpace(templateName))
            throw new ArgumentException("Template name is required.", nameof(templateName));

        if (string.IsNullOrWhiteSpace(templateContent))
            throw new ArgumentException("Template content is required.", nameof(templateContent));

        var template = new DocumentTemplate(
            DefaultIdType.NewGuid(),
            templateName,
            documentType,
            templateContent);

        return template;
    }

    /// <summary>
    /// Updates the template content.
    /// </summary>
    public DocumentTemplate Update(
        string? templateContent = null,
        string? templateVariables = null,
        string? description = null)
    {
        if (!string.IsNullOrWhiteSpace(templateContent))
        {
            TemplateContent = templateContent;
            Version++;
        }

        if (templateVariables != null)
            TemplateVariables = templateVariables;

        if (description != null)
            Description = description;

        return this;
    }

    /// <summary>
    /// Deactivates the template.
    /// </summary>
    public DocumentTemplate Deactivate()
    {
        IsActive = false;
        return this;
    }

    /// <summary>
    /// Activates the template.
    /// </summary>
    public DocumentTemplate Activate()
    {
        IsActive = true;
        return this;
    }
}

/// <summary>
/// Document type constants.
/// </summary>
public static class DocumentTypeNames
{
    public const string EmploymentContract = "EmploymentContract";
    public const string OfferLetter = "OfferLetter";
    public const string Separation = "Separation";
    public const string Payslip = "Payslip";
    public const string TaxForm = "TaxForm";
    public const string BenefitsForm = "BenefitsForm";
    public const string Handbook = "Handbook";
    public const string Policy = "Policy";
    public const string Other = "Other";
}

