using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain.Events;

/// <summary>
/// Domain events for Reporting and Communication entities.
/// </summary>
/// 
// ReportDefinition Events
public sealed record ReportDefinitionCreated(ReportDefinition Report) : DomainEvent;
public sealed record ReportDefinitionUpdated(ReportDefinition Report) : DomainEvent;
public sealed record ReportScheduleConfigured(Guid ReportId, string Frequency) : DomainEvent;
public sealed record ReportScheduleDisabled(Guid ReportId) : DomainEvent;
public sealed record ReportDefinitionActivated(Guid ReportId) : DomainEvent;
public sealed record ReportDefinitionDeactivated(Guid ReportId) : DomainEvent;

// ReportGeneration Events
public sealed record ReportGenerationQueued(ReportGeneration Generation) : DomainEvent;
public sealed record ReportGenerationStarted(Guid GenerationId) : DomainEvent;
public sealed record ReportGenerationCompleted(Guid GenerationId, string OutputFile, int RecordCount) : DomainEvent;
public sealed record ReportGenerationFailed(Guid GenerationId, string ErrorMessage) : DomainEvent;
public sealed record ReportGenerationCancelled(Guid GenerationId) : DomainEvent;

// CommunicationTemplate Events
public sealed record CommunicationTemplateCreated(CommunicationTemplate Template) : DomainEvent;
public sealed record CommunicationTemplateUpdated(CommunicationTemplate Template) : DomainEvent;
public sealed record CommunicationTemplateActivated(Guid TemplateId) : DomainEvent;
public sealed record CommunicationTemplateDeactivated(Guid TemplateId) : DomainEvent;

// CommunicationLog Events
public sealed record CommunicationQueued(CommunicationLog Communication) : DomainEvent;
public sealed record CommunicationSent(Guid CommunicationId, string Recipient) : DomainEvent;
public sealed record CommunicationDelivered(Guid CommunicationId, string Recipient) : DomainEvent;
public sealed record CommunicationFailed(Guid CommunicationId, string Recipient, string ErrorMessage) : DomainEvent;
public sealed record CommunicationBounced(Guid CommunicationId, string Recipient, string? Reason) : DomainEvent;
public sealed record CommunicationOpened(Guid CommunicationId, string Recipient) : DomainEvent;
