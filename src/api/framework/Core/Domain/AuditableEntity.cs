using System.ComponentModel.DataAnnotations.Schema;
using FSH.Framework.Core.Domain.Contracts;

namespace FSH.Framework.Core.Domain;

/// <summary>
/// Base auditable entity using the project's default identifier type.
/// </summary>
/// <remarks>
/// Provides common auditing and soft-delete properties shared across most domain entities.
/// This concrete, non-generic type simply specializes <see cref="AuditableEntity{TId}"/> with the
/// application's <c>DefaultIdType</c> (typically a System.Guid alias).
/// </remarks>
public abstract class AuditableEntity : AuditableEntity<DefaultIdType>
{
    /// <summary>
    /// Initializes a new instance of <see cref="AuditableEntity"/> and assigns a new Id.
    /// </summary>
    protected AuditableEntity() => Id = DefaultIdType.NewGuid();
}

/// <summary>
/// Generic auditable entity base that includes common metadata fields used for auditing and soft deletes.
/// </summary>
/// <typeparam name="TId">The identifier type used by the entity (for example, <see cref="DefaultIdType"/>).</typeparam>
/// <remarks>
/// Use this class as the base for domain entities that need common fields such as Name, Description, ImageUrl,
/// CreatedOn/LastModifiedOn timestamps, and soft-delete tracking. This centralizes audit fields and keeps
/// entity models consistent across the application.
/// </remarks>
public class AuditableEntity<TId> : BaseEntity<TId>, IAuditable, ISoftDeletable
{
    /// <summary>
    /// Display name for the entity. Maximum column length defined by the mapping attribute.
    /// Default: empty string.
    /// </summary>
    [Column(TypeName = "VARCHAR(1024)")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Optional short description of the entity.
    /// </summary>
    [Column(TypeName = "VARCHAR(2048)")]
    public string? Description { get; set; }

    /// <summary>
    /// Optional extended notes about the entity for internal use.
    /// </summary>
    [Column(TypeName = "VARCHAR(2048)")]
    public string? Notes { get; set; }

    /// <summary>
    /// Optional publicly consumable image URL for the entity. Typically points to a CDN or file store.
    /// </summary>
    /// <remarks>
    /// Stored as a string (URL). Consumers should store absolute URIs when possible so clients can load images directly.
    /// Maximum length and validation rules can be enforced at the application layer.
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1056:URI-like properties should not be strings", Justification = "Persisting URL as string for DB compatibility and cross-project serialization; callers should use absolute URIs when possible.")]
    public string? ImageUrl { get; set; }

    /// <summary>
    /// UTC timestamp when the entity was created.
    /// </summary>
    public DateTimeOffset CreatedOn { get; set; }

    /// <summary>
    /// Identifier of the user who created the entity.
    /// </summary>
    public DefaultIdType CreatedBy { get; set; }

    /// <summary>
    /// Optional username of the creator for quick reference in logs or UI.
    /// </summary>
    [Column(TypeName = "VARCHAR(64)")]
    public string? CreatedByUserName { get; set; }

    /// <summary>
    /// UTC timestamp when the entity was last modified.
    /// </summary>
    public DateTimeOffset LastModifiedOn { get; set; }

    /// <summary>
    /// Identifier of the user who last modified the entity (if any).
    /// </summary>
    public DefaultIdType? LastModifiedBy { get; set; }

    /// <summary>
    /// Optional username of the last modifier for quick reference.
    /// </summary>
    [Column(TypeName = "VARCHAR(64)")]
    public string? LastModifiedByUserName { get; set; }

    /// <summary>
    /// UTC timestamp when the entity was soft-deleted (if applicable).
    /// </summary>
    public DateTimeOffset? DeletedOn { get; set; }

    /// <summary>
    /// Identifier of the user who deleted the entity (if applicable).
    /// </summary>
    public DefaultIdType? DeletedBy { get; set; }

    /// <summary>
    /// Optional username of the deleter for quick reference.
    /// </summary>
    [Column(TypeName = "VARCHAR(64)")]
    public string? DeletedByUserName { get; set; }
}

/// <summary>
/// Convenience base class for auditable entities that include an approval workflow payload.
/// </summary>
/// <remarks>
/// This class is a non-generic specialization of <see cref="AuditableEntityWithApproval{TId}"/> using
/// the default application identifier type.
/// </remarks>
public abstract class AuditableEntityWithApproval : AuditableEntityWithApproval<DefaultIdType>;

/// <summary>
/// Extends <see cref="AuditableEntity{TId}"/> with common approval-related fields used by workflows.
/// </summary>
/// <typeparam name="TId">The identifier type used by the entity (for example, <see cref="DefaultIdType"/>).</typeparam>
/// <remarks>
/// Adds fields such as Request, Feedback, Status, Remarks and audit references for prepared/reviewed/approved actors.
/// Useful for domain objects that go through multi-step approval or review processes.
/// </remarks>
public abstract class AuditableEntityWithApproval<TId> : AuditableEntity<TId>
{
    /// <summary>
    /// Optional JSON or textual request payload submitted for approval.
    /// </summary>
    [Column(TypeName = "VARCHAR(1024)")]
    public string? Request { get; set; }

    /// <summary>
    /// Optional short feedback string provided by reviewers.
    /// </summary>
    [Column(TypeName = "VARCHAR(32)")]
    public string? Feedback { get; set; }

    /// <summary>
    /// Current status of the approval workflow (for example "Pending", "Approved", "Rejected").
    /// </summary>
    [Column(TypeName = "VARCHAR(32)")]
    public string? Status { get; set; }

    /// <summary>
    /// Optional remarks or notes about the approval decision.
    /// </summary>
    [Column(TypeName = "VARCHAR(1024)")]
    public string? Remarks { get; set; }
    
    /// <summary>
    /// Identifier of the user who prepared the request.
    /// </summary>
    public DefaultIdType? PreparedBy { get; set; }

    /// <summary>
    /// Readable name of the preparer.
    /// </summary>
    [Column(TypeName = "VARCHAR(1024)")]
    public string? PreparerName { get; set; }

    /// <summary>
    /// When the request was prepared.
    /// </summary>
    public DateTime? PreparedOn { get; set; }

    /// <summary>
    /// Identifier of the reviewer who inspected the request.
    /// </summary>
    public DefaultIdType? ReviewedBy { get; set; }

    /// <summary>
    /// Readable name of the reviewer.
    /// </summary>
    [Column(TypeName = "VARCHAR(1024)")]
    public string? ReviewerName { get; set; }

    /// <summary>
    /// When the request was reviewed.
    /// </summary>
    public DateTime? ReviewedOn { get; set; }

    /// <summary>
    /// Identifier of the user who recommended the request.
    /// </summary>
    public DefaultIdType? RecommendedBy { get; set; }

    /// <summary>
    /// Readable name of the recommender.
    /// </summary>
    [Column(TypeName = "VARCHAR(1024)")]
    public string? RecommenderName { get; set; }

    /// <summary>
    /// When the request was recommended.
    /// </summary>
    public DateTime? RecommendedOn { get; set; }

    /// <summary>
    /// Identifier of the approver for the request.
    /// </summary>
    public DefaultIdType? ApprovedBy { get; set; }

    /// <summary>
    /// Readable name of the approver.
    /// </summary>
    [Column(TypeName = "VARCHAR(1024)")]
    public string? ApproverName { get; set; }

    /// <summary>
    /// When the request was approved.
    /// </summary>
    public DateTime? ApprovedOn { get; set; }
}
